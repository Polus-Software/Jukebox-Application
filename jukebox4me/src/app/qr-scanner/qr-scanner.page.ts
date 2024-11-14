import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Barcode, BarcodeScanner } from '@capacitor-mlkit/barcode-scanning';
import { AlertController } from '@ionic/angular';

@Component({
    selector: 'app-qr-scanner',
    templateUrl: './qr-scanner.page.html',
    styleUrls: ['./qr-scanner.page.scss'],
})
export class QrScannerPage implements OnInit {

    isSupported = false;
    barcodes: Barcode[] = [];

    constructor(private alertController: AlertController, private router: Router) { }

    ngOnInit() {
        BarcodeScanner.isSupported().then((result) => {
            this.isSupported = result.supported;
        });
    }

    async startScan(): Promise<void> {
        const granted = await this.requestPermissions();
        if (!granted) {
            this.presentAlert();
            return;
        }

        const { barcodes } = await BarcodeScanner.scan();

        if (barcodes.length > 0) {
            const displayValue = barcodes[0].displayValue.replace(/\s/g, '');
            const params = new URLSearchParams(displayValue);
            const accountId = params.get('accountId');
            const zoneId = params.get('zoneId');
            if (accountId && zoneId) {
                localStorage.clear();
                localStorage.setItem('zoneId', zoneId);
                localStorage.setItem('accountId', accountId);
                this.router.navigate(['/home']);
            }
        } else {
            console.log('No barcodes detected.');
        }
    }


    async requestPermissions(): Promise<boolean> {
        const { camera } = await BarcodeScanner.requestPermissions();
        return camera === 'granted' || camera === 'limited';
    }

    async presentAlert(): Promise<void> {
        const alert = await this.alertController.create({
            header: 'Permission denied',
            message: 'Please grant camera permission to use the barcode scanner.',
            buttons: ['OK'],
        });
        await alert.present();
    }
}