namespace UbytkacAdmin.Properties {

    internal sealed partial class Settings {

        public Settings() {
            // // Pro přidávání obslužných rutin událostí určených pro ukládání a změnu nastavení
            // odkomentujte prosím níže uvedené řádky:
            //
            // this.SettingChanging += this.SettingChangingEventHandler;
            //
            // this.SettingsSaving += this.SettingsSavingEventHandler;
        }

        private void SettingChangingEventHandler(object sender, System.Configuration.SettingChangingEventArgs e) {
            // Kód pro zpracování události SettingChangingEvent přidejte sem.
        }

        private void SettingsSavingEventHandler(object sender, System.ComponentModel.CancelEventArgs e) {
            // Kód pro zpracování události SettingsSaving přidejte sem.
        }
    }
}