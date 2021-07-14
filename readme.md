 docker image build --tag makepdf .
 docker container run --rm --name htmltopdf -p 8080:8080 makepdf