#!/bin/bash

echo '---------------- Setup Inicial -----------------'
echo '---------------- Criando a fila ----------------'
awslocal sqs create-queue --queue-name messagebus-queue

echo '--------------- Enviando mensagens -------------'
awslocal sqs send-message \
--queue-url http://localhost:4566/000000000000/messagebus-queue \
--message-body '{'Id':'988FAF58-AC86-4123-8919-86D57AD4A6FA','Name':'Tiago','LastName':'Pala'}'