<template>
    <div class="dropdown">
        <button class="btn btn-secondary dropdown-toggle"
                type="button"
                id="dropdownMenuClickableInside"
                data-bs-toggle="dropdown"
                data-bs-auto-close="outside"
                aria-expanded="false">
            <b>{{ inputAdult }} <i class="fas fa-user"></i></b>
            <b>{{ inputChild }} <i class="fas fa-baby"></i></b>
<!--             <b>{{ inputRoom }} <i class="fas fa-door-closed"></i></b> -->
        </button>
        <ul class="dropdown-menu" aria-labelledby="dropdownMenuButton1">
            <div class="container text-start">
                <p>
                    {{ $t('labels.adults') }}
                    <InputNumber v-model="inputAdult"
                                 showButtons
                                 :min="0"
                                 :max="100"
                                 value="0" />
                </p>
                <p>
                    {{ $t('labels.children') }}
                    <InputNumber v-model="inputChild"
                                 showButtons
                                 :min="0"
                                 :max="100"
                                 value="0" />
                </p>
<!--                 <p>
                    {{ $t('labels.roomss') }}
                    <InputNumber v-model="inputRoom"
                                 showButtons
                                 :min="0"
                                 :max="100"
                                 value="0" />
                </p> -->
            </div>
        </ul>
    </div>
</template>

<script>
import InputNumber from "primevue/inputnumber";

export default {
    components: {
        InputNumber,
    },
    computed: {
        inputAdult: {
            get() {
                return this.$store.state.bookingDetail.adultInput;
            },
            set(value) {
                this.$store.state.bookingDetail.adultInput = value;
            },
        },
        inputChild: {
            get() {
                return this.$store.state.bookingDetail.childrenInput;
            },
            set(value) {
                this.$store.state.bookingDetail.childrenInput = value;
            },
        },
        // inputRoom: {
        //     get() {
        //         return this.$store.state.searchString.inputRooms;
        //     },
        //     set(value) {
        //         this.$store.dispatch("updateRoom", value);
        //     },
        // },
    }
}

</script>

<style scoped>
.dropdown-toggle{
  background-color: rgb(83 193 110);
  border: #14a04d;
}

.btn-secondary:hover {
  background-color: rgb(83 193 110);
  border: #14a04d;
}

b{
  padding-left: 10px;
}



</style>