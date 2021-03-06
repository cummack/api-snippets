require('dotenv').load();

const http = require('http');
const path = require('path');

const AccessToken = require('twilio').AccessToken;
const ConversationsGrant = AccessToken.ConversationsGrant;
const express = require('express');
const randomUsername = require('./randos');

// Create Express webapp
const app = express();
app.use(express.static(path.join(__dirname, 'public')));

app.get('/token', (request, response) => {
  const identity = randomUsername();

  // Create an access token which we will sign and return to the client,
  // containing the grant we just created
  const token = new AccessToken(
    process.env.TWILIO_ACCOUNT_SID,
    process.env.TWILIO_API_KEY,
    process.env.TWILIO_API_SECRET
  );

  // Assign the generated identity to the token.
  token.identity = identity;

  // Grant access to Video.
  const grant = new VideoGrant();
  grant.configurationProfileSid = process.env.TWILIO_CONFIGURATION_SID;
  token.addGrant(grant);

  // Serialize the token to a JWT string and include it in a JSON response
  response.send({
    identity: identity,
    token: token.toJwt(),
  });
});

// Create http server and run it
const server = http.createServer(app);
const port = process.env.PORT;

server.listen(port = 3000, function() {
  console.log('Express server running on *:' + port);
});
