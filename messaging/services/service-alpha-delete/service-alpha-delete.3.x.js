// Download the Node helper library from twilio.com/docs/libraries/node
// These consts are your accountSid and authToken from https://www.twilio.com/console
const accountSid = 'ACXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX';
const authToken = 'your_auth_token';

const client = require('twilio')(accountSid, authToken);
const service = client.messaging.services('MG2172dd2db502e20dd981ef0d67850e1a');

service.alphaSenders('AIc781610ec0b3400c9e0cab8e757da937')
       .remove()
       .then(function(response) {
         console.log(response);
       }).catch(function(error) {
         console.log(error);
       });
