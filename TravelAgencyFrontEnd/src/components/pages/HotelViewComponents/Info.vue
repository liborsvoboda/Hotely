<template>
  <div class="py-4">
      <div class="p-4 bg-light rounded shadow-sm">
          <h5>{{ $t('labels.ratings') }}: <span class="badge rounded-pill bg-secondary">{{hotel.averageRating}}</span></h5>
          <div class="col-lg-12 col-md-12" v-html="($store.state.language == 'cz') ? hotel.descriptionCz : hotel.descriptionEn" />
          <h4>{{ $t('labels.servicesAndProperties') }}:</h4>

          <div class="amentities">
              <p v-for="property in valueProperties" :title="(property.fee) ? (property.feeValue != null) ? $t('labels.fee') + ' ' + property.feeValue + ' ' + hotel.defaultCurrency.name : $t('labels.fee') + ' ' + property.feeRangeMin + ' - ' + property.feeRangeMax + ' ' + hotel.defaultCurrency.name : ''">
                  {{property.name}}:
                  <span class="badge rounded-pill bg-secondary">
                      {{ property.value }} {{ property.unit }}
                  </span>
              </p>

              <p v-for="property in bitProperties" :title="(property.fee) ? (property.feeValue != null) ? $t('labels.fee') + ' ' + property.feeValue + ' ' + hotel.defaultCurrency.name : $t('labels.fee') + ' ' + property.feeRangeMin + ' - ' + property.feeRangeMax + ' ' + hotel.defaultCurrency.name : ''">
                  <i class="fas fa-check"></i> {{property.name}}
              </p>

          </div>
      </div>
  </div>
</template>

<script>
export default {
    computed: {
        hotel() {
            return this.$store.state.hotel;
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
    mounted() {
        if (this.$store.state.searchString.dates.length && this.$store.state.searchString.dates[1] != null) { 
            this.$store.dispatch("getReservedRoomList", this.hotel.id);
        }
        
    },
};
</script>

<style scoped>

 div{
   padding-top: 30px;
 }

 div.amentities{
   text-align:justify;
   margin-left:12cm;
 }
</style>