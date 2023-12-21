<template>
    <div class="row container d-flex align-items-center justify-content-center">
        <div class="col-md-12">
            <div class="">
                <!-- class="profile-content" -->
                <router-view></router-view>
            </div>
        </div>
    </div>
</template>

<script>
export default {
    data() {
        return {
        };
    },
    computed: {
        loggedIn() {
            return this.$store.state.user.loggedIn;
        },
        user() {
            return this.$store.state.user;
        },

    },
}

</script>

<style scoped>
    .router-link-exact-active {
        font-weight: bold !important;
        font-size: 20px !important;
        color: green !important;
    }

    .nav {
        display: block;
    }
    /* Profile container */
    .profile {
        margin: 20px 0;
    }

    /* Profile sidebar */
    .profile-sidebar {
        padding: 20px 0 20px 0;
        background: #fff;
        border-radius: 30px;
        position: relative;
        right: 15px;
    }

    .profile-userpic img {
        float: none;
        margin: 0 auto;
        width: 70%;
        height: 70%;
        -webkit-border-radius: 50% !important;
        -moz-border-radius: 50% !important;
        border-radius: 50% !important;
    }

    .profile input {
        /* padding-left:30px; */
    }

    .profile-usertitle {
        text-align: center;
        margin-top: 20px;
    }

    .profile-usertitle-name {
        color: #5a7391;
        font-size: 16px;
        font-weight: 600;
        margin-bottom: 7px;
    }

    .profile-usertitle-job {
        text-transform: uppercase;
        color: #5b9bd1;
        font-size: 12px;
        font-weight: 600;
        margin-bottom: 15px;
    }

    .profile-userbuttons {
        position: relative;
        right: 15px;
        margin-top: 10px;
    }

        .profile-userbuttons .btn {
            font-size: 14px;
            padding: 6px 12px;
            margin-right: 5px;
        }

            .profile-userbuttons .btn:last-child {
                margin-right: 0px;
            }

    .profile-usermenu {
        margin-top: 30px;
    }

    .profile-userbuttons ul {
        display: block;
    }

    .profile-usermenu ul li {
        border: 1px solid #f0f4f7;
        /* border-top: 2px solid #f0f4f7;
            border-bottom: 2px solid #f0f4f7; */
    }

        /* .profile-usermenu ul li:last-child {
            border-bottom: none;
        } */

        .profile-usermenu ul li a {
            color: #93a3b5;
            font-size: 14px;
            font-weight: 400;
            /* display: block; */
        }

            .profile-usermenu ul li a i {
                margin-right: 8px;
                font-size: 14px;
                /* display: block; */
            }

            .profile-usermenu ul li a:hover {
                background-color: #fafcfd;
                color: #5b9bd1;
            }

        .profile-usermenu ul li.active {
            border-bottom: none;
        }

            .profile-usermenu ul li.active a {
                color: #5b9bd1;
                background-color: #f6f9fb;
                border-left: 2px solid #5b9bd1;
                margin-left: -2px;
            }

    /* Profile Content */
    .profile-content {
        /* padding: 20px; */
        background: #fff;
        min-height: 800px;
/*         min-width: 217mm; */
        border-radius: 30px;
    }
</style>

