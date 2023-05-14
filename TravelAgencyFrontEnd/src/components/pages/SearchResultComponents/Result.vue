<template>
  <div class="p-4 rounded shadow-sm">
    <div id="testOmega">
      <div class="row">
        <div class="col-md-4">
          <PhotoSlider :photos="photos" :width="'210px'" :height="'150px'" :key="hotel.id"/>
        </div>
        <div class="col-md-8">
          <div class="row">
              <div class="col-md-6 text-start">
                  <b>{{ hotel.name }}</b>
                  <p>{{ hotel.city.city }}, {{ hotel.country.systemName }}</p>
                  <p>
                      {{ $t('labels.ratings') }}:
                      <span class="badge rounded-pill bg-secondary">
                          {{ hotel.averageRating }}
                      </span>
                  </p>
                  <p v-for="property in valueProperties" :title="(property.fee) ? (property.feeValue != null) ? $t('labels.fee') + ' ' + property.feeValue + ' ' + hotel.defaultCurrency.name : $t('labels.fee') + ' ' + property.feeRangeMin + ' - ' + property.feeRangeMax + ' ' + hotel.defaultCurrency.name : ''">
                      {{property.name}}:
                      <span class="badge rounded-pill bg-secondary">
                          {{ property.value }} {{ property.unit }}
                      </span>
                  </p>
              </div>

            <div class="col-md-6 text-start">
              <h5>
                  <small>{{ $t('labels.roomPriceFrom') }}:</small> <b>{{ lowestPrice }} {{ hotel.defaultCurrency.name }}</b>
              </h5>
              <br />

              <p v-for="property in bitProperties" :title="(property.fee) ? (property.feeValue != null) ? $t('labels.fee') + ' ' + property.feeValue + ' ' + hotel.defaultCurrency.name : $t('labels.fee') + ' ' + property.feeRangeMin + ' - ' + property.feeRangeMax + ' ' + hotel.defaultCurrency.name : ''">
                  <i class="fas fa-check"></i> {{property.name}}
              </p>

            </div>
          </div>
          <router-link :to="'/hotels/' + hotel.id" class="nav-link">
              <button class="btn btn-primary"
                      for="btn-check-outlined"
                      @click="hotelDetailsClick">
                  {{ $t('labels.seeDetail') }}
              </button><br />
          </router-link>
        </div>
      </div>
    </div>
  </div>
</template>


<script>
import Info from "../HotelViewComponents/Info.vue";
import PhotoSlider from "../HotelViewComponents/RoomsViewComponents/PhotoSlider.vue";

export default {
    components: {
        Info,
        PhotoSlider,
    },
    computed: {
        lowestPrice() {
            var rooms = this.hotel.hotelRoomLists;
            rooms.sort((a, b) => {
            return a.price - b.price;
            });
            var min = rooms[0];
            return min.price;
        },
        photos() {
            var photos = [];
            photos.push({ id: this.hotel.id, hotelPhoto:this.$store.state.apiRootUrl + '/Image/' + this.hotel.id + '/' + this.hotel.hotelImagesLists.filter(obj => { return obj.isPrimary === true; })[0].fileName });

            this.hotel.hotelImagesLists.forEach(image => {
                if (!image.isPrimary) { photos.push({ id: this.hotel.id, hotelPhoto: this.$store.state.apiRootUrl + '/Image/' + this.hotel.id + '/' + image.fileName }) }
            });
            return photos;
        },
        valueProperties() {
            var valueProperties = [];
            this.hotel.hotelPropertyAndServiceLists.forEach(property => {
                var valueProperty = this.$store.state.propertyList.filter(obj => { return obj.id === property.propertyOrServiceId; })[0];
                if (property.isAvailable && property.approved && valueProperty.isValue) { valueProperties.push({ name: valueProperty.systemName, value: property.value, unit: valueProperty.propertyOrServiceUnitType.systemName, fee: property.fee, feeValue: property.feeValue, feeRangeMin: property.feeRangeMin, feeRangeMax: property.feeRangeMax }); }
            });
            return valueProperties;
        },
        bitProperties() {
            var bitProperties = [];
            this.hotel.hotelPropertyAndServiceLists.forEach(property => {
                var valueProperty = this.$store.state.propertyList.filter(obj => { return obj.id === property.propertyOrServiceId; })[0];
                if (property.isAvailable && property.approved && valueProperty.isBit) { bitProperties.push({ name: valueProperty.systemName, fee: property.fee, feeValue: property.feeValue, feeRangeMin: property.feeRangeMin, feeRangeMax: property.feeRangeMax }); }
            });
            return bitProperties;
        }

    },
    props: {
        hotel: {},
    },
    methods: {
        hotelDetailsClick(event) {
            this.$store.dispatch("setHotel", this.hotel);
        },
    },
};
</script>

<style scoped>
#testOmega {
  opacity: 100% !important;
}

.btn.btn-primary{
    background-color:#53c16e;
    border-color:#1bc541;
  }

a.nav-link {
  margin-left: 35mm;
}

.p-4 {
  margin-top: 15px;
  background-color: rgb(241, 241, 241);
}
</style>