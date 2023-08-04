<template>
  <div class="row">
      <div class="col-md-3">
          <div v-for="property,index in propertyList">
              <div v-if="property.propertyGroupId != null &&(index -1 == -1 || ((index -1 >= 0 && propertyList[index -1].propertyGroupId != property.propertyGroupId)))" class="accordion accordion-flush" :id="'menuname'+property.propertyGroupId">
                  <div class="accordion-item">
                      <h2 class="accordion-header" id="flush-headingOne">
                          <button class="accordion-button collapsed"
                                  type="button"
                                  data-bs-toggle="collapse"
                                  :data-bs-target="'#menu'+property.propertyGroupId"
                                  aria-expanded="false"
                                  :aria-controls="'menu'+property.propertyGroupId">
                              {{ property.propertyGroup.systemName }}
                          </button>
                      </h2>
                      <div :id="'menu'+property.propertyGroupId"
                           class="accordion-collapse collapse"
                           aria-labelledby="flush-headingOne"
                           :data-bs-parent="'#menuname'+property.propertyGroupId">
                          <div class="accordion-body text-start">

                              <div v-for="subproperty in propertyList">
                                  <div v-if="subproperty.propertyGroupId == property.propertyGroupId && subproperty.isValue">
                                      <p>{{subproperty.systemName}}</p>
                                      <Slider v-model="subproperty.searchDefaultValue"
                                              :id="'prop'+subproperty.id"
                                              :step="0.1"
                                              :min="subproperty.searchDefaultMin"
                                              :max="subproperty.searchDefaultMax"
                                              :format="(subproperty.propertyOrServiceUnitType.systemName == 'Km') ? kmFormat: null"
                                              @change="checkFilters(subproperty.id,subproperty.searchDefaultValue)">
                                      </Slider>
                                      <hr />
                                  </div>
                                  <div v-else-if="subproperty.propertyGroupId == property.propertyGroupId && subproperty.isBit" class="form-check">
                                      <input class="form-check-input"
                                             v-model="subproperty.searchDefaultBit"
                                             type="checkbox"
                                             :id="'prop'+subproperty.id"
                                             @click="checkFilters(subproperty.id,subproperty.searchDefaultBit)" />
                                      <label class="form-check-label">
                                          {{subproperty.systemName}}
                                      </label>
                                  </div>
                              </div>
                          </div>
                      </div>
                  </div>
              </div>
          </div>

          <div id="sliders">
              <div>
                  <div class="accordion accordion-flush" id="accordionFlushExample">
                      <div class="accordion-item">
                          <h2 class="accordion-header" id="flush-headingOne">
                              <button class="accordion-button collapsed"
                                      type="button"
                                      data-bs-toggle="collapse"
                                      data-bs-target="#flush-collapseOne"
                                      aria-expanded="false"
                                      aria-controls="flush-collapseOne">
                                  {{ $t('labels.moreFilters') }}
                              </button>
                          </h2>
                          <div id="flush-collapseOne"
                               class="accordion-collapse collapse"
                               aria-labelledby="flush-headingOne"
                               data-bs-parent="#accordionFlushExample">
                              <div class="accordion-body text-start">
                                  <div v-for="property in propertyList">
                                      <div v-if="property.isValue && property.propertyGroupId == null">
                                          <p>{{property.systemName}}</p>
                                          <Slider v-model="property.searchDefaultValue"
                                                  :id="'prop'+property.id"
                                                  :step="0.1"
                                                  :min="property.searchDefaultMin"
                                                  :max="property.searchDefaultMax"
                                                  :format="(property.propertyOrServiceUnitType.systemName == 'Km') ? kmFormat: null"
                                                  @change="checkFilters(property.id,property.searchDefaultValue)"></Slider>
                                          <hr />
                                      </div>
                                      <div v-else-if="property.isBit && property.propertyGroupId == null" class="form-check">
                                          <input class="form-check-input"
                                                 v-model="property.searchDefaultBit"
                                                 type="checkbox"
                                                 value=""
                                                 :id="'prop'+property.id"
                                                 @click="checkFilters(property.id,property.searchDefaultBit)" />
                                          <label class="form-check-label">
                                              {{property.systemName}}
                                          </label>
                                      </div>
                                  </div>
                              </div>
                          </div>
                      </div>
                  </div>
              </div>
          </div>
      </div>

        <div v-if="filteredHotels.length" class="col-md-9">
            <Result v-for="result in filteredHotels"
                    :hotel="result.hotel"
                    :key="result.hotel.id" />
        </div>
    </div>
</template>
<script>
// code: https://www.vuescript.com/custom-range-slider/
import Slider from "/node_modules/@vueform/slider";
import Info from "../HotelViewComponents/Info.vue";
import Result from "./Result.vue";
import Card from "primevue/card";
export default {
    components: {
        Slider,
        Info,
        Result,
        Card,
    },
    data() {
        return {
            kmFormat: {
                decimals: 1,
                suffix: " Km",
            },
            pricerange: {
                value: [0, 2000],
            },
            // beachDistance: {
            //   value: 7,
            //   step: -1,
            //   format: {
            //     decimals: 1,
            //     suffix: " km",
            //   },
            // },
        };
    },
    computed: {
        propertyList() {
            return this.$store.state.propertyList;
        },
        filteredResults() {
            return this.$store.state.filteredResults;
        },
        hotelsCount() {
            return this.filteredHotels.length;
        },
        filteredHotels() {
            if (this.filteredResults.length) {
                this.$emit("updateNrOfHotels", this.filteredResults.length);
                return this.filteredResults;
            }
            this.$emit("updateNrOfHotels", 0);
            return 0;
        },
    },
    methods: {
        checkFilters(id,value) {
            this.$store.state.filteredResults = [];

            this.$store.state.searchResults.forEach(hotel => {
                let allowed = true;

                this.propertyList.forEach(prop => {

                    //check enabled bit properties - bit value paradox clicked value sending oposite value
                    if (allowed && prop.isBit && ((prop.id != id && prop.searchDefaultBit) || (prop.id == id && !prop.searchDefaultBit))) {
                        allowed = false;
                        hotel.hotel.hotelPropertyAndServiceLists.forEach(hotelProp => {
                            if (prop.id == hotelProp.propertyOrServiceId && hotelProp.isAvailable) { allowed = true; }
                        });
                    }

                    //check enabled value properties
                    if (allowed && prop.isValue && prop.searchDefaultValue > 0) {
                        allowed = false;
                        hotel.hotel.hotelPropertyAndServiceLists.forEach(hotelProp => {
                            if (prop.id == hotelProp.propertyOrServiceId && hotelProp.isAvailable && hotelProp.value != null && prop.searchDefaultValue >= hotelProp.value) { allowed = true; }
                        });
                    }
                    
                });

                if (allowed) { this.$store.state.filteredResults.push(hotel); }
            });
        }
    }

};
</script>

<style src="@vueform/slider/themes/default.css">
</style>

<style scoped>

div.col-md-4{
  padding-right: 25px;
}

.myContainer {
  width: 300%;
  height: auto;
  margin: 10px;
  padding: 10px;
  display: grid;
}
.hotelResults {
  width: 30%;
  height: fit-content;
  margin: auto;
  margin-right: 5px;
}
.myFilter {
  width: 101%;
  margin-left: 5px;
  position: absolute;
}
#sliders {
  color: rgb(0, 0, 0);
  }


#accordionFlushExample{
  border:solid;
  border-color: silver;
  margin-bottom: 25px;
}

.accordion {
  color: black;
}

</style>
