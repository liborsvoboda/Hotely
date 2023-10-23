<template>
   <!DOCTYPE html>
<html>

<head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Untitled</title>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/4.1.3/css/bootstrap.min.css">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/ionicons/2.0.1/css/ionicons.min.css">
</head>

<body style="min-height:auto !important;">
    <div class="footer-basic">
        <footer>
            <div class="social p-0 m-0">
                <a href="#"><i class="icon ion-social-instagram"></i></a>
                <a href="#"><i class="icon ion-social-snapchat"></i></a>
                <a href="#"><i class="icon ion-social-twitter"></i></a>
                <a href="#"><i class="icon ion-social-facebook"></i></a>
            </div>
            <ul class="list-inline">
                <li class="list-inline-item"><a href="#"><router-link to="/">{{ $t('labels.home') }}</router-link></a></li>
                <li class="list-inline-item"><a href="/Services">{{ $t('labels.services') }}</a></li>
                <li class="list-inline-item"><router-link to="/About">{{ $t('labels.about') }}</router-link></li>
                <li class="list-inline-item"><router-link to="/Terms">{{ $t('labels.termsConditions') }}</router-link></li>
                <li class="list-inline-item"><router-link to="/PrivacyPolicy">{{ $t('labels.privacyPolicy') }}</router-link></li>
            </ul>
            <p class="copyright p-0 m-0">copyright CoolNET © 2023 by <a href="https://groupware-solution.eu" class="no-wrap" target="_blank">GroupWare-Solution.Eu</a> použito <a href="https://KlikneteZde.Cz:5000" class="no-wrap" target="_blank">EASY-DATA-Center Řešení</a> & <a href="https://metroui.org.ua" target="_blank">Metro4</a></p>
        </footer>
    </div>
</body>

</html>
</template>

<script>

</script>


<style scoped>
.footer-basic {
  padding:10px 0;
  background-color:rgb(51, 57, 58);
  color:#f3f3f3;
}

.footer-basic ul {
  padding:0;
  list-style:none;
  text-align:center;
  font-size:18px;
  line-height:1.6;
  margin-bottom:0;
}

.footer-basic li {
  padding:0 10px;
}

.footer-basic ul a {
  color:inherit;
  text-decoration:none;
  opacity:0.8;
}

.footer-basic ul a:hover {
  opacity:1;
}

.footer-basic .social {
  text-align:center;
  padding-bottom:10px;
}

.footer-basic .social > a {
  font-size:24px;
  width:40px;
  height:40px;
  line-height:40px;
  display:inline-block;
  text-align:center;
  border-radius:50%;
  border:1px solid #ccc;
  margin:0 8px;
  color:inherit;
  opacity:0.75;
}

.footer-basic .social > a:hover {
  opacity:0.9;
}

.footer-basic .copyright {
  margin-top:5px;
  text-align:center;
  font-size:13px;
  color:#aaa;
  margin-bottom:0;
}

</style>