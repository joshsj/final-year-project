// Auth0 Custom Action

const namespace = "https://rendezvous/claim";

accessToken.setCustomClaim(`${namespace}/name`, name);
accessToken.setCustomClaim(`${namespace}/email`, email);
