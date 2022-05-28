 insert into Uzytkownik(imie, nr_tel, email, haslo, dolaczono) values 
 ('Pawe³', '111111111', 'pawel.a.brandt@gmail.com', '$2a$11$7hCVBBqf0PoE2eE9WYSmiuf0mmuAGpse/rjCe3QpaenwqSNb7IQfS', '2022-04-03'),
 ('Ola', '222222222', 'ag@op.pl', '$2a$11$7hCVBBqf0PoE2eE9WYSmiuf0mmuAGpse/rjCe3QpaenwqSNb7IQfS', '2022-04-03'),
 ('Kuba', '333333333', 'ks@wp.pl', '$2a$11$7hCVBBqf0PoE2eE9WYSmiuf0mmuAGpse/rjCe3QpaenwqSNb7IQfS', '2022-04-03'),
 ('Janusz', '444444444', 'jp@wp.pl', '$2a$11$7hCVBBqf0PoE2eE9WYSmiuf0mmuAGpse/rjCe3QpaenwqSNb7IQfS', '2022-04-03'),
 ('Tomasz', '555555555', 'mmt@gmail.com', '$2a$11$7hCVBBqf0PoE2eE9WYSmiuf0mmuAGpse/rjCe3QpaenwqSNb7IQfS', '2022-04-03');

 insert into [Admin](login, haslo) values ('admin', '$2a$11$7hCVBBqf0PoE2eE9WYSmiuf0mmuAGpse/rjCe3QpaenwqSNb7IQfS');
 
 insert into Kategoria(nazwa, nadkategoria_id) values
 ('Motoryzacja', null),
 ('Elektronika', null),
 ('Moda', null),
 ('Rozrywka', null),
 ('Dom', null),
 ('Zwierzêta', null),
 ('Sport', null),
 ('Edukacja', null);

 insert into Ogloszenie(wystawil_id, kategoria_id, nazwa, opis, aktywne, dodano, typ, cena, negocjacja, pokaz_tel, pokaz_email) values
 (1, 2, 'Praca jako Programista ASP.NET', 'Szukam pracy jako programista ASP.NET C#. Mam 5 letnie doœwiadczenie w owej technologii.', 1, '2022-04-7', 'Poszukiwanie pracy', 5100.0, 1, 1, 1),
 (2, 3, 'Buty Nike Air max 97', 'Buty s¹ w idealnym stanie.', 1, '2022-04-8', 'Oferta sprzeda¿y', 150.0, 0, 1, 1),
 (3, 1, 'Kupiê samochód', 'Co najmniej 200KM i silnik benzyna.', 1, '2022-04-8', 'Oferta kupna', 11000.0, 1, 0, 1),
 (1, 4, 'Zamieniê Pokemon Legends Arceus', 'Wymieniê siê gr¹ Pokemon Legends Arceus za nowego Kangurka Kao na konsolê Nintendo Switch.', 1, '2022-04-9', 'Oferta wymiany', 0.0, 1, 1, 0),
 (2, 2, 'Szukam Web Developera', 'Szukam web developera ¿eby wykona³ dla mnie stronê internetow¹.', 1, '2022-04-10', 'Oferta pracy', 12000.0, 1, 1, 1),
 (4, 5, 'Oddam Rega³', 'Rega³ u¿ywany przez parê lat ale stan wci¹¿ bardzo dobry.', 1, '2022-04-16', 'Oferta sprzeda¿y', 0.0, 0, 1, 1),
 (5, 5, 'Ogrodzenia do domu', 'Prowadzê firmê ogrodzeniow¹ od 10 lat. Mo¿liwa wycena przez telefon. Zapraszam do kontaktu.', 1, '2022-04-17', 'Oferta sprzeda¿y', 0.0, 0, 1, 0),
 (5, 2, 'Szukam Grafika 3D', 'Poszukujê grafika aby wykona³ modele 3D ogrodzeñ.', 1, '2022-04-20', 'Oferta pracy', 2500.0, 1, 1, 0),
 (4, 2, 'safwwersg', 'ddrthjyrt.', 1, '2022-04-22', 'Oferta pracy', 2500000.0, 0, 0, 0),
 (4, 6, 'Oddam szczeniaki w dobre rêce', 'Mam 6 ma³ych szczeniaków psa rasy Golden Retriever. Aby upewniæ siê ¿e oddane zostan¹ w dobre rêce przyjadê do pañstwa ze szczeniakiem, a po tygodniu i miesi¹cu jeszcze raz sprawdziæ jak siê szczeniak trzyma.', 1, '2022-04-23', 'Oferta sprzeda¿y', 5500.0, 1, 1, 1),
 (3, 7, 'Rêkawice bramkarskie', 'Kupiê rêkawice bramkarskie, nie u¿ywane marki Adidas.', 1, '2022-04-27', 'Oferta kupna', 30.0, 1, 0, 0),
 (2, 8, 'Ksi¹¿ki Po³o¿nicze', 'Sprzedam podrêczniki na studia o kierunku po³o¿nictwo.', 1, '2022-05-01', 'Oferta sprzeda¿y', 2500.0, 1, 1, 0),
 (4, 8, 'Podrêczniki szkolne', 'Kupiê podrêczniki dla 5 klasy szko³y podstawowej.', 1, '2022-04-20', 'Oferta pracy', 300.0, 1, 1, 1);
 
 insert into Chat(ogloszenie_id, zainteresowany_id, oglaszajacy_id) values
 (5, 1, 2),
 (3, 4, 3);

 insert into Wiadomosc(chat_id, nadawca_id, wiadomosc, wyslano) values
 (1, 1, 'Dzieñ dobry, tak siê sk³ada, ¿e mam du¿e doœwiadczenie w pisaniu stron internetowych. Wys³aæ Pani CV?', '2022-04-11 15:24:04'),
 (1, 2, 'Dzieñ dobry, oczywiœcie, proszê przes³aæ na maila podanego w og³oszeniu.', '2022-04-11 15:26:04'),
 (1, 2, 'Ju¿ przesy³am CV. Pozdrawiam :)', '2022-04-11 15:29:04'),
 (1, 2, 'Dziêkujê. Równie¿ pozdrawiam :)', '2022-04-11 15:31:04'),
 (2, 4, 'Mam do sprzedania Audi A4 250KM 2.0 benzyna, czarny lakier', '2022-04-20 09:29:04'),
 (2, 3, 'Ju¿ nieaktualne', '2022-04-21 11:01:04');
 
 insert into Ocena(ocena_od_id, ocena_dla_id, ocena) values
 (1, 2, 5),
 (2, 1, 5),
 (4, 3, 3),
 (3, 4, 4);

 insert into Ulubiona(uzytkownik_id, ogloszenie_id) values
 (1, 5),
 (1, 8),
 (1, 10),
 (2, 1),
 (2, 6),
 (2, 10);

 insert into Zgloszenie(Ogloszenie_id, zglaszajacy_id, zgloszony_id, tresc) values 
 (9, 1, 4, 'Tytu³ i treœæ og³oszenia nie ma ¿adnego sensu.') ,
 (9, 3, 4, '¿adnych informacji');
