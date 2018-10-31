# Mobile Inc (Unity)
IF3111 Pengembangan Aplikasi pada Platform Khusus

## Anggota kelompok Chlordane
- [Reinhard Benjamin Linardi (13515011)](https://github.com/reinhardlinardi)
- [Erick Wijaya (13515057)](https://github.com/wijayaerick)
- [Roland Hartanto (13515107)](https://github.com/rolandhartanto)

## Deskripsi subsistem Unity

Subsistem Unity merupakan game simulasi pengelolaan sumber daya, dimana pemain berperan sebagai manajer dari sebuah e-commerce yang menjual handphone. Subsistem ini terdiri dari 3 scene, yaitu menu, kantor, dan gudang. Pada  scene menu, pengguna dapat memulai game baru maupun melanjutkan game sebelumnya. Pada scene kantor, pengguna dapat mengelola uang, menerima pesanan, mempekerjakan karyawan, dll. Pada scene gudang, pengguna dapat mengirim barang pesanan.

## Fitur-fitur subsistem Unity

- Subsistem menerapkan hukum fisika sederhana berupa gravitasi
- Subsistem memiliki objek yang dapat digerakkan oleh mouse (drag and drop)
- Subsistem memiliki *sound effect*
- Subsistem memiliki 3 scene, yaitu menu, kantor, dan gudang
- Subsistem memanfaatkan kamera
- Subsistem mengimplementasikan animasi terhadap objek AC dan matahari
- Subsistem memiliki lebih dari 2 sumber cahaya dan salah satunya memiliki animasi berupa perubahan intensitas, yaitu matahari
- Subsistem menggunakan *prefabs* sebagai komponen interaktifnya
- Subsistem memanfaatkan *canvas* dan *panel* untuk *layouting* antarmuka
- Subsistem memanfaatkan *PlayerPrefs*
- Subsistem memanfaatkan basis data SQLite
- Subsistem dapat melakukan integrasi dengan subsistem lain dengan memanfaatkan *networking*, seperti mengirim kode promosi
- Subsistem yang dibuat di-deploy dalam aplikasi desktop

## Cara instalasi aplikasi
1. Download dan extract ZIP dari [MobileInc-Unity](https://github.com/tugas-itb-erick/MobileInc-Unity/releases). 
2. Run aplikasi (.exe). 

## Panduan pemakaian aplikasi

1. Run aplikasi. User dapat memilih Start untuk memulai game baru atau memilih Load untuk melanjutkan game.
2. Jika user memilih Start, maka user harus memasukkan username. Username ini harus unik, jika tidak user tidak dapat memulai game. Jika user memilih Load, maka user dapat memilih username yang tersedia.
3. User akan dibawa ke scene kantor. Pada game ini ada parameter money, happiness, trust, popularity, dan target. Target adalah jumlah uang untuk menang yang harus dicapai user dalam waktu tertentu. Money adalah uang yang dimiliki user, sedangkan happiness, trust, dan popularity dapat mempengaruhi jumlah pesanan yang diterima user. User juga dapat mempekerjakan karyawan baru serta mengirim kode promosi.
4. User dapat berpindah ke scene gudang. Pada scene gudang, user dapat mengetahui perkiraan jumlah stok barang. User dapat membeli barang lagi jika stok barang tidak cukup. Untuk memenuhi pesanan, user dapat mengambil barang dari gudang, memasukkan ke box dan mengirimnya. Jika user tidak berhasil memenuhi pesanan dalam waktu tertentu, maka akan mempengaruhi trust dari pemesan.
    
<br />
<br />Homepage : http://mobileinc.herokuapp.com
<br />API server : https://github.com/reinhardlinardi/mobile-inc
