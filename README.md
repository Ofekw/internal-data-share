# Project Coevolution

Data sharing solution for Fiserv

## Usage

### Backend

#### Build for Deployment
The backend is self contained and currently hosted on an azure server. There are not install requirements. Once the frontend is built, It will automatically interface with the backend server.

#### Build for Development
If you wish to run the backend standalone, the internal-data-share repo can be cloned (master branch), the solution opened in visual studio, then the project launched. HTTP requests can then be sent to the documented API's.

### Frontend

#### Build for Deployment

```
cd frontend
npm install
npm install -g webpack
webpack
```

You can now serve the frontend directory.

#### Build for Development

```
cd frontend
npm install
npm install -g webpack-dev-server
webpack-dev-server --hot
```

Open a browser and navigate to <http://localhost:8080/>. Your changes will be rebuilt automatically.
