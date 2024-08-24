run:
	docker compose up -d

watch:
	docker compose watch

destroy:
	docker compose down


destroy-image: destroy
	docker-compose down --rmi all -v --remove-orphans

runb-server: destroy
	docker compose up server --build -d