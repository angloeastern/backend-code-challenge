run:
	docker compose up -d

watch:
	docker compose watch

destroy:
	docker compose down


destroy-image: destroy
	docker-compose down --rmi all -v --remove-orphans

recreate-server: destroy
	docker compose up server --build -d

test:
	docker build -t dotnet-docker-image-test --progress=plain --no-cache --target test .

recreate-all: destroy destroy-image run