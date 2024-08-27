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

rr:
	docker compose restart server

test:
	docker compose exec -w /source/AEBackend.Tests server  dotnet test --logger "console;verbosity=detailed"


recreate-all: destroy destroy-image run