require 'nokogiri'

module LanguageHandler
  class XmlMatcher
    def self.match(xml_str1, xml_str2)
      self.new(xml_str1).match(xml_str2)
    end

    def initialize(xml_str)
      @xml = canonical_xml(xml_str)
    end

    def match(xml_str)
      target_xml = canonical_xml(xml_str)
      match_nodes [@xml.root, target_xml.root]
    end

    private

    def canonical_xml(xml_str)
      Nokogiri::XML.parse(Nokogiri::XML(xml_str).canonicalize)
    end

    def match_nodes(nodes)
      node1, node2 = nodes
      node1.name == node2.name &&
        node1.node_type == node2.node_type &&
        match_node_attributes(node1, node2) &&
        match_node_contents(node1, node2)
    end

    def match_node_contents(node1, node2)
      if node1.type == Nokogiri::XML::Node::TEXT_NODE
        sanitize_text(node1.text) == sanitize_text(node2.text)
      else
        match_node_children(node1, node2)
      end
    end

    def sanitize_text(text)
      text.gsub(/\n|\r|\t/, ' ').strip.squeeze(' ')
    end

    def match_node_children(node1, node2)
      children1 = sanitize_children(node1)
      children2 = sanitize_children(node2)

      children1.length == children2.length &&
        children1.zip(children2).all?(&method(:match_nodes))
    end

    def sanitize_children(node)
      node.children.to_a.reject(&:blank?)
    end

    def match_node_attributes(node1, node2)
      node_attributes_hash(node1) == node_attributes_hash(node2)
    end

    def node_attributes_hash(node)
      node.attribute_nodes.map { |attr| [attr.name, attr.value] }.to_h
    end
  end
end
