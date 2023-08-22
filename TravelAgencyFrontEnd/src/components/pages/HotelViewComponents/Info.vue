<template>
    <div class="py-4">
        <div class="p-4 bg-light rounded shadow-sm">
            <h5>{{ $t('labels.ratings') }}: <span class="rounded-pill">{{hotel.averageRating}}</span></h5>
            <div class="col-lg-12 col-md-12" v-html="($store.state.language == 'cz') ? hotel.descriptionCz : hotel.descriptionEn" />
            <h4>{{ $t('labels.servicesAndProperties') }}:</h4>

            <div class="col-lg-12 col-md-12 d-flex pt-0 pl-0 pr-0">
                <div class="col-md-2 pt-0 pl-0 pr-0" v-for="propertyGroup,index in propertyGroupList">
                    <b>{{propertyGroup.systemName}}</b>

                    <p v-for="property in valueProperties" style="margin-bottom:0px;" :title="(property.fee) ? (property.feeValue != null) ? $t('labels.fee') + ' ' + property.feeValue + ' ' + hotel.defaultCurrency.name : $t('labels.fee') + ' ' + property.feeRangeMin + ' - ' + property.feeRangeMax + ' ' + hotel.defaultCurrency.name : ''">
                        <span v-if="property.propertyGroupId == propertyGroup.id" style="font-size:12px;">
                            {{property.name}}:
                            <span class="rounded-pill">
                                {{ property.value }} {{ property.unit }}
                            </span>
                        </span>
                    </p>

                    <p v-for="property in bitProperties" style="margin-bottom:0px;" :title="(property.fee) ? (property.feeValue != null) ? $t('labels.fee') + ' ' + property.feeValue + ' ' + hotel.defaultCurrency.name : $t('labels.fee') + ' ' + property.feeRangeMin + ' - ' + property.feeRangeMax + ' ' + hotel.defaultCurrency.name : ''">
                        <span v-if="property.propertyGroupId == propertyGroup.id" style="font-size:12px;">
                            <i class="fas fa-check"></i> {{property.name}}
                        </span>
                    </p>
                </div>
            </div>


            <div class="amentities">
                <div class="col-lg-12 col-md-12 d-flex pt-0 pl-0 pr-0">
                    <div class="col-md-4 pt-0 text-center">
                        <b>{{ $t('labels.moreFilters') }}</b>
                    </div>
                </div>
                <p v-for="property in valueProperties" style="margin-bottom:0px;" :title="(property.fee) ? (property.feeValue != null) ? $t('labels.fee') + ' ' + property.feeValue + ' ' + hotel.defaultCurrency.name : $t('labels.fee') + ' ' + property.feeRangeMin + ' - ' + property.feeRangeMax + ' ' + hotel.defaultCurrency.name : ''">
                    <span v-if="property.propertyGroupId == null">
                        {{property.name}}:
                        <span class="badge rounded-pill bg-secondary">
                            {{ property.value }} {{ property.unit }}
                        </span>
                    </span>
                </p>

                <p v-for="property in bitProperties" style="margin-bottom:0px;" :title="(property.fee) ? (property.feeValue != null) ? $t('labels.fee') + ' ' + property.feeValue + ' ' + hotel.defaultCurrency.name : $t('labels.fee') + ' ' + property.feeRangeMin + ' - ' + property.feeRangeMax + ' ' + hotel.defaultCurrency.name : ''">
                    <span v-if="property.propertyGroupId == null">
                        <i class="fas fa-check"></i> {{property.name}}
                    </span>
                </p>

            </div>
        </div>
    </div>
</template>

<script>
export default {
    computed: {
        propertyGroupList() {
            return this.$store.state.propertyGroupList;
        },
        hotel() {
            return this.$store.state.hotel;
        },
        valueProperties() {
            var valueProperties = [];
                    
            this.hotel.hotelPropertyAndServiceLists.forEach(property => {
                var valueProperty = this.$store.state.propertyList.filter(obj => { return obj.id === property.propertyOrServiceId; })[0];
                if (property.isAvailable && valueProperty.isValue) { valueProperties.push({ propertyGroupId: valueProperty.propertyGroupId, name: valueProperty.systemName, value: property.value, unit: valueProperty.propertyOrServiceUnitType.systemName, fee: property.fee, feeValue: property.feeValue, feeRangeMin: property.feeRangeMin, feeRangeMax: property.feeRangeMax }); }
            });
            return valueProperties;
        },
        bitProperties() {
            var bitProperties = [];
            this.hotel.hotelPropertyAndServiceLists.forEach(property => {
                var valueProperty = this.$store.state.propertyList.filter(obj => { return obj.id === property.propertyOrServiceId; })[0];
                if (property.isAvailable && valueProperty.isBit) { bitProperties.push({ propertyGroupId: valueProperty.propertyGroupId, name: valueProperty.systemName, fee: property.fee, feeValue: property.feeValue, feeRangeMin: property.feeRangeMin, feeRangeMax: property.feeRangeMax }); }
            });
            return bitProperties;
        }
    },
    mounted() {
        this.$store.dispatch("getPropertyGroupList");

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