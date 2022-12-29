### How to run the backend locally
1. Install Docker
2. (Not needed atm) Run docker container for redis: docker run --name cryptid-redis -d -p 6379:6379 redis:latest
3. Run docker container for postgresql: docker run --name cryptid-postgres -e POSTGRES_USER=admin -e POSTGRES_PASSWORD=secret -e POSTGRES_DB=CryptidDb -d -p 5432:5432 postgres
4. Add user to the postgresql: CREATE database "Cryptid_Backend" USE Cryptid_Backend CREATE LOGIN CryptidUser WITH PASSWORD = 'Test1234' Create USER CryptidUser FOR LOGIN CryptidUser GRANT ALTER TO CryptidUser GRANT control TO CryptidUser
5. Run API project

## How to run the client
1. Go to the MainScene
2. Find ConnectionController
3. Set use remote to false
4. Set urls

### How to deploy it to the azure
1. Go to azure devops: **TODO: Add link**
2. Go to azure pipelines and click run specific pipeline
3. Check the hash on the server website link