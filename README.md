# BackEnd
Backend en netcore

Backend orientado a microservicos, por ende, tiene una capa intermedia de ApiGateway hecha con ocelot (https://ocelot.readthedocs.io/en/latest/introduction/bigpicture.html).
El acceso a datos esta con Code First, y tiene todos los migrations correspondientes.
Para autenticar hay un proyecto de Single Sign On (SSO), desarrollado bajo el protocolo OpenID Connect (https://openid.net/connect/).
Aquí se utiliza el framework IdentityServer (http://docs.identityserver.io/en/latest/), que es una implementación de OpenId Connect y OAuth 2.0.

