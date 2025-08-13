<template>
    <div class="">
        <div class="rounded drop-shadow row">
            <div class="col-md-6 text-left">
                <h1>{{ $t('labels.creditPackages') }}</h1>
            </div>
            <div class="col-md-6 text-right">
                <span class="icon mif-info pt-3 mif-3x c-pointer fg-orange" onclick="OpenDocView('CreditPackages')" />
            </div>
        </div>
        <hr>
        <div class="card-body">

            <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12 pl-5 pr-5 pt-5 mb-0">
                <ul data-role="tabs" data-expand="true" data-on-tab="setBackgroundProfileMenu()">
                    <!-- <ul data-app-bar="true" data-role="materialtabs" data-fixed-tabs="true" data-deep="true"> -->
                    <li id="creditPackagesMenu" class="fg-black text-bold bg-brandColor1"><a href="#_creditPackagesMenu">{{ $t('labels.creditPackages') }}</a></li>
                    <li id="yourCredits" class="fg-black text-bold"><a href="#_yourCredits">{{ $t('labels.yourCredits') }}</a></li>
                    <li id="creditsSettings" class="fg-black text-bold"><a href="#_creditsSettings">{{ $t('labels.creditsSettings') }}</a></li>
                    <li id="creditHistory" class="fg-black text-bold"><a href="#_creditHistory">{{ $t('labels.creditHistory') }}</a></li>
                </ul>
            </div>

            <div id="_creditPackagesMenu">
                <div class="d-flex row gutters ml-5 mr-5 mb-5 border">
                    <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                        <h6 class="mt-3 mb-2 text-primary">{{ $t('labels.creditPackages') }}</h6>
                    </div>

                    <div class="cardcontainer">

                        <!--  TODO foreach -->
                        <div class="cardbox p-5" v-for="credit in CreditPackages">
                            <div class="face face1">
                                <div class="content">
                                    <img src="/src/assets/img/sketch.svg" style="height: 100px;width: auto;" alt="">
                                    <h3>{{ credit.name }}</h3>
                                </div>
                            </div>
                            <div class="face face2">
                                <p>
                                    {{ credit.description }}
                                </p>
                                <a class="c-pointer" href="#" @click="payProcessStart(credit.systemName,credit.creditPrice)">{{window.dictionary('labels.buyCredit') }}</a>
                            </div>
                        </div>
                    </div>

                    <div class="d-flex row gutters m-5 border">
                        <form id="payment-form">
                            <div id="card-element">
                                <!--Stripe.js injects the Card Element-->
                            </div>
                            <button id="submit" class="btn btn-primary stripe-btn">
                                <div class="spinner hidden" id="spinner"></div>
                                <span id="button-text">Pay now</span>
                            </button>
                            <p id="card-error" role="alert"></p>
                            <p class="result-message hidden">
                                Payment succeeded, finishing booking.
                            </p>
                        </form>
                    </div>
                </div>
            </div>
        </div>


        <div id="_yourCredits">
            <div class="d-flex row gutters ml-5 mr-5 mb-5 border">
                <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                    <h6 class="mt-3 mb-2 text-primary">{{ $t('labels.yourCredits') }}</h6>
                </div>
            </div>
        </div>

        <div id="_creditsSettings">
            <div class="d-flex row gutters ml-5 mr-5 mb-5 border">
                <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                    <h6 class="mt-3 mb-2 text-primary">{{ $t('labels.creditsSettings') }}</h6>
                </div>
            </div>
        </div>

        <div id="_creditHistory">
            <div class="d-flex row gutters ml-5 mr-5 mb-5 border">
                <div class="col-xl-12 col-lg-12 col-md-12 col-sm-12 col-12">
                    <h6 class="mt-3 mb-2 text-primary">{{ $t('labels.creditHistory') }}</h6>
                </div>
            </div>
        </div>


<!-- 
        <div class="row gutters pr-5">
            <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12">
            </div>
            <div class="col-xl-6 col-lg-6 col-md-6 col-sm-6 col-12">
                <div class="text-right">
                    <button type="button" class="button secondary outline shadowed mb-1" @click="resetForm();">
                        {{ $t('user.cancelChanges') }}
                    </button>
                    <button type="button" class="button success outline shadowed ml-1 mb-1" @click="checkPasswords()">
                        {{ $t('user.saveChanges') }}
                    </button>
                    <button type="button" class="button alert outline shadowed ml-1 mb-1" @click="deleteAccout()">
                        {{ $t('user.deleteAccount') }}
                    </button>
                </div>
            </div>
        </div>
 -->

    </div>
</template>

<script>

var purchase = {
    items: [{ id: "xl-tshirt" }],
    };
    var stripe = Stripe(
    "pk_test_51PKf8802zAptyzT6njFfMNWSvnh2SCTunbeC50JRZJtDFVI19pijzkx0t3watJ7PgL1n5dYEJA0MHlJce09iPTkz00pTdbBeGX"
    );
    var elements = stripe.elements();
    var card = undefined;
    // Set up Stripe.js and Elements to use in checkout form
    var style = {
        base: {
            color: "#32325d",
            fontFamily: '"Helvetica Neue", Helvetica, sans-serif',
            fontSmoothing: "antialiased",
            fontSize: "16px",
            "::placeholder": {
                color: "#32325d",
            },
        },
        invalid: {
            color: "#fa755a",
            iconColor: "#fa755a",
        },
    };

    import Card from "primevue/card";

export default {
    components: { Card },
    data() {
        return {
            AdvertiserSetingChanged: false,
        };
    },
    computed: {
        loggedIn() {
            return this.$store.state.user.loggedIn;
        },
        user() {
            this.guest.UserId = this.$store.state.user.UserId == "" ? false : true;
            return this.$store.state.user;
        },
        CreditPackages() {
            return this.$store.state.creditPackages;
        }
    },
    async mounted() {
        if (card === undefined) {
            card = elements.create("card", { style: style });
        }
        card.mount("#card-element");
    },
    methods: {
        async getCreditPackages() {
            await this.dispatch('getCreditPackages');
        },
        payWithCard(stripe, card, clientSecret) {
            var ref = this;
            var email = this.user.Email;
            stripe.confirmCardPayment(clientSecret, {
                    receipt_email: email,
                    payment_method: {
                        card: card,
                    },
                })
                .then(function (result) {
                    if (result.error) {
                        // Show error to your customer
                        showError(result.error.message);
                    } else {
                        // The payment succeeded!
                        orderComplete(result.paymentIntent.id);
                        console.log("Payment Confirmed!");
                        // ref.createBooking();
                    }
                });
        },
        payProcessStart(itemName,price) {
            var ref = this;
            fetch("http://localhost:5000/WebApi/Credits/PaymentProcess", {
                method: "POST",
                headers: { "Content-Type": "application/json", },
                body: JSON.stringify({ items: [{ id: itemName }], price: price}),
            }).then(function (result) {
                    return result.json();
                }).then(function (data) {
                    card.on("change", function (event) {
                        // Disable the Pay button if there are no card details in the Element
                        // document.querySelector("button").disabled = event.empty;
                        // document.querySelector("#card-error").textContent = event.error ? event.error.message : "";
                    });
                    var form = document.getElementById("payment-form");
                    form.addEventListener("submit", function (event) {
                        event.preventDefault();
                        // Create the order here. Make sure it is completed!

                        // Complete payment when the submit button is clicked
                        ref.payWithCard(stripe, card, data.clientSecret);
                    });
                });
        }
    },
    async created() {
        await this.$store.dispatch('getCreditPackages');
    }
};
</script>

<style scoped>
label {
  color: black;
}

.text {
  color: red;
}

#update{
  background-color: rgb(83 193 110);
  border: #0e833d
}
</style>
