<template>
    <div class="container">
        <div class="row">

            <!-- <a v-for="photo in photos" :href="photo.hotelPhoto" data-toggle="lightbox" data-gallery="gallery" class="col-md-4" target="_blank">
                <div class="img-container drop-shadow ">
                    <img :src="photo.hotelPhoto" class="img-fluid rounded">
                </div>
            </a> -->
            <div data-role="lightbox">
                <img v-for="photo in photos" :src="photo.hotelPhoto" :data-original="photo.hotelPhoto" class="c-pointer drop-shadow m-2" style="max-width:250px;" />
            </div>
        </div>
    </div>
</template>



<style scoped>
.row {
    margin: 15px;
}
</style>


<script>
export default {
    data() {

    },
    computed: {
        photos() {
            var photos = [];
            photos.push({ id: this.hotel.id, hotelPhoto: this.$store.state.apiRootUrl + '/Image/' + this.hotel.id + '/' + this.hotel.hotelImagesLists.filter(obj => { return obj.isPrimary === true; })[0].fileName });

            this.hotel.hotelImagesLists.forEach(image => {
                if (!image.isPrimary) { photos.push({ id: this.hotel.id, hotelPhoto: this.$store.state.apiRootUrl + '/Image/' + this.hotel.id + '/' + image.fileName }) }
            });
            this.hotel.hotelRoomLists.forEach(room => {
                photos.push({ id: this.hotel.id, hotelPhoto: this.$store.state.apiRootUrl + '/RoomImage/' + room.id })
            });
            return photos;
        },
        hotel() {
            return this.$store.state.hotel;
        }
    },
}
</script>


