# Get twilio-ruby from twilio.com/docs/ruby/install
require 'twilio-ruby'

# Get your Account Sid and Auth Token from twilio.com/user/account
account_sid = 'ACXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX'
auth_token = 'your_auth_token'

# Initialize Twilio Client
@client = Twilio::REST::Client.new(account_sid, auth_token)

# Get the accounts with the given sid
@accounts = @client.api.v2010.accounts(account_sid)

# Create a call and after the call has begun pass the given digits
call = @accounts.calls.create(
  url: 'http://demo.twilio.com/docs/voice.xml',
  to: '+14155551212',
  send_digits: '1234#',
  from: '+18668675309',
  method: 'GET'
)

# Print the call start time
puts call.start_time
