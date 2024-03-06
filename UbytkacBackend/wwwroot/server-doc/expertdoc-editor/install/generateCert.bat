@ECHO off
@ECHO Replace localhost with your domain name, example:  somedomain.com

REM openssl req -x509 -newkey rsa:4096 -sha256 -keyout openssl.key -out openssl.crt -subj “/CN=localhost” -days 600
openssl req -x509 -newkey rsa:4096 -sha256 -keyout openssl.key -out openssl.crt -days 600 -config cfgfile.cnf

openssl pkcs12 -export -name “localhost.co.nz” -out openssl.pfx -inkey openssl.key -in openssl.crt
openssl req -newkey rsa:2048 -new -nodes -x509 -days 3650 -keyout key.pem -out localhost.cert.pem