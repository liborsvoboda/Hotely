# TravelAgencyFrontEnd

1] ApiUrl is set in 
store/index.js parameter> apiRootUrl: 'http://localhost:5000'
and use variable: store.state.apiRootUrl


2] Run local for debuging
   npm vite

3] Build Production
	npm vite build
	and copy dist folder to Server web root directory

4] Allowed Path are in: router/index.js

5] Api call example
await fetch(
          'http://nomad.ubytkac.cz:5000/api/Search/search?input=' + searchString
        )
