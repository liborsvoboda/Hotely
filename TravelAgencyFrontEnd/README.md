# TravelAgencyFrontEnd

1] ApiUrl is in 
store/index.js parameter> apiRootUrl: 'http://localhost:5000'
and use variable: store.state.apiRootUrl
is Setted By Prod/Dev running in srv/main.js


2] Run local for debuging
	cd .\TravelAgencyFrontEnd
    npm run dev
    running on http://localhost:3000/

3] Build Production
	npm run build
	and copy dist folder to Server web root directory

4] Allowed Path are in: router/index.js

5] Api call example
await fetch(
          this.state.apiRootUrl + '/Search/search?input=' + searchString
        )
All Api are called from store/index.js + HotelView.vue, Registration.vue
  Task move from vue to store/index.js

6] Sringify Object
    JSON.stringify(credentials)

7] i18n in HTML: {{ $t('labels.locationObject') }}
    i18n in code: this.$i18n.t("messages.passwordsNotMatch");

8] Logged UserData state.user

9] store variables in vue files>  this.$store.state.some

10] Dispatch Then  action
    await this.$store.dispatch('registration', regInfo).then(() => {

    setHotelSearchResultsList - onsahuje vracene inzeraty

    
    použit camesCase
    DictionaryKeyPolicy = JsonNamingPolicy.CamelCase, PropertyNamingPolicy = JsonNamingPolicy.CamelCase});

    image checker
    https://codebeautify.org/base64-to-image-converter

11)  pøeklad {{ $t('user.login') }} :label="$t('labels.reviews')"
  
12] language condition

{{($store.state.language == 'cz') ? hotel.descriptionCz : hotel.descriptionEn}}


    computed: {
        language() {
            return this.$store.state.language;
        }
    },

<div v-if="language =='cz' " class="col-lg-9 col-md-9">
                        {{ ubytkacInfo.descriptionCz }}
                    </div>

13) show html content

<div class="col-lg-12 col-md-12" v-html="($store.state.language == 'cz') ? hotel.descriptionCz : hotel.descriptionEn" />

14) search date to cz format
 //let startDate
          //let endDate
          //if (this.state.searchString.dates.length) {
          //    startDate = this.state.searchString.dates[0].toLocaleDateString('cs-CZ')
          //    endDate = this.state.searchString.dates[1].toLocaleDateString('cs-CZ')
          //}

15] scrolování na obrazovce
window.scrollTo(0,0)


