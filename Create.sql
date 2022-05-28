CREATE TABLE Uzytkownik (
  id INTEGER  NOT NULL   IDENTITY ,
  imie VARCHAR(30)    ,
  nr_tel VARCHAR(15)    ,
  email VARCHAR(100)    ,
  haslo VARCHAR(100)    ,
  dolaczono DATE      ,
PRIMARY KEY(id));
GO




CREATE TABLE Usunieci (
  id INTEGER  NOT NULL   IDENTITY ,
  imie VARCHAR(30)    ,
  nr_tel VARCHAR(15)    ,
  email VARCHAR(100)    ,
  haslo VARCHAR(100)    ,
  dolaczono DATE    ,
  usunieto DATE      ,
PRIMARY KEY(id));
GO




CREATE TABLE Kategoria (
  id INTEGER  NOT NULL   IDENTITY ,
  nadkategoria_id INTEGER NULL  ,
  nazwa VARCHAR(50)      ,
PRIMARY KEY(id)  ,
  FOREIGN KEY(nadkategoria_id)
    REFERENCES Kategoria(id));
GO


CREATE INDEX Kategoria_FKIndex1 ON Kategoria (nadkategoria_id);
GO


CREATE INDEX IFK_Rel_01 ON Kategoria (nadkategoria_id);
GO


CREATE TABLE Admin (
  id INTEGER  NOT NULL   IDENTITY ,
  login VARCHAR(100)  NOT NULL  ,
  haslo VARCHAR(100)  NOT NULL    ,
PRIMARY KEY(id));
GO




CREATE TABLE Ocena (
  id INTEGER  NOT NULL   IDENTITY ,
  ocena_od_id INTEGER  NOT NULL  ,
  ocena_dla_id INTEGER  NOT NULL  ,
  ocena FLOAT      ,
PRIMARY KEY(id)    ,
  FOREIGN KEY(ocena_dla_id)
    REFERENCES Uzytkownik(id),
  FOREIGN KEY(ocena_od_id)
    REFERENCES Uzytkownik(id));
GO


CREATE INDEX Ocena_FKIndex1 ON Ocena (ocena_dla_id);
GO
CREATE INDEX Ocena_FKIndex2 ON Ocena (ocena_od_id);
GO


CREATE INDEX IFK_Rel_10 ON Ocena (ocena_dla_id);
GO
CREATE INDEX IFK_Rel_11 ON Ocena (ocena_od_id);
GO


CREATE TABLE Ogloszenie (
  id INTEGER  NOT NULL   IDENTITY ,
  wystawil_id INTEGER  NOT NULL  ,
  kategoria_id INTEGER  NOT NULL  ,
  nazwa VARCHAR(50)    ,
  opis VARCHAR(500)    ,
  aktywne BIT    ,
  dodano DATETIME    ,
  typ VARCHAR(100)    ,
  cena FLOAT    ,
  negocjacja BIT    ,
  pokaz_tel BIT    ,
  pokaz_email BIT      ,
PRIMARY KEY(id)    ,
  FOREIGN KEY(kategoria_id)
    REFERENCES Kategoria(id),
  FOREIGN KEY(wystawil_id)
    REFERENCES Uzytkownik(id));
GO


CREATE INDEX Ogloszenie_FKIndex1 ON Ogloszenie (wystawil_id);
GO
CREATE INDEX Ogloszenie_FKIndex2 ON Ogloszenie (kategoria_id);
GO


CREATE INDEX IFK_Rel_02 ON Ogloszenie (kategoria_id);
GO
CREATE INDEX IFK_Rel_12 ON Ogloszenie (wystawil_id);
GO


CREATE TABLE Zgloszenie (
  id INTEGER  NOT NULL   IDENTITY ,
  Ogloszenie_id INTEGER  NOT NULL  ,
  zglaszajacy_id INTEGER  NOT NULL  ,
  zgloszony_id INTEGER  NOT NULL  ,
  tresc VARCHAR(500)      ,
PRIMARY KEY(id)      ,
  FOREIGN KEY(zglaszajacy_id)
    REFERENCES Uzytkownik(id),
  FOREIGN KEY(zgloszony_id)
    REFERENCES Uzytkownik(id),
  FOREIGN KEY(Ogloszenie_id)
    REFERENCES Ogloszenie(id));
GO


CREATE INDEX Zgloszenie_FKIndex1 ON Zgloszenie (zglaszajacy_id);
GO
CREATE INDEX Zgloszenie_FKIndex2 ON Zgloszenie (zgloszony_id);
GO
CREATE INDEX Zgloszenie_FKIndex3 ON Zgloszenie (Ogloszenie_id);
GO


CREATE INDEX IFK_Rel_13 ON Zgloszenie (zglaszajacy_id);
GO
CREATE INDEX IFK_Rel_14 ON Zgloszenie (zgloszony_id);
GO
CREATE INDEX IFK_Rel_15 ON Zgloszenie (Ogloszenie_id);
GO


CREATE TABLE Chat (
  id INTEGER  NOT NULL   IDENTITY ,
  ogloszenie_id INTEGER  NOT NULL  ,
  zainteresowany_id INTEGER  NOT NULL  ,
  oglaszajacy_id INTEGER  NOT NULL    ,
PRIMARY KEY(id)      ,
  FOREIGN KEY(ogloszenie_id)
    REFERENCES Ogloszenie(id),
  FOREIGN KEY(oglaszajacy_id)
    REFERENCES Uzytkownik(id),
  FOREIGN KEY(zainteresowany_id)
    REFERENCES Uzytkownik(id));
GO


CREATE INDEX Chat_FKIndex1 ON Chat (oglaszajacy_id);
GO
CREATE INDEX Chat_FKIndex2 ON Chat (zainteresowany_id);
GO
CREATE INDEX Chat_FKIndex3 ON Chat (ogloszenie_id);
GO


CREATE INDEX IFK_Rel_03 ON Chat (ogloszenie_id);
GO
CREATE INDEX IFK_Rel_04 ON Chat (oglaszajacy_id);
GO
CREATE INDEX IFK_Rel_05 ON Chat (zainteresowany_id);
GO


CREATE TABLE Wiadomosc (
  id INTEGER  NOT NULL   IDENTITY ,
  chat_id INTEGER  NOT NULL  ,
  nadawca_id INTEGER  NOT NULL  ,
  wiadomosc VARCHAR(500)    ,
  wyslano DATETIME      ,
PRIMARY KEY(id)    ,
  FOREIGN KEY(nadawca_id)
    REFERENCES Uzytkownik(id),
  FOREIGN KEY(chat_id)
    REFERENCES Chat(id));
GO


CREATE INDEX Wiadomosc_FKIndex1 ON Wiadomosc (nadawca_id);
GO
CREATE INDEX Wiadomosc_FKIndex2 ON Wiadomosc (chat_id);
GO


CREATE INDEX IFK_Rel_06 ON Wiadomosc (nadawca_id);
GO
CREATE INDEX IFK_Rel_07 ON Wiadomosc (chat_id);
GO


CREATE TABLE Ulubiona (
  id INTEGER  NOT NULL   IDENTITY ,
  uzytkownik_id INTEGER  NOT NULL  ,
  ogloszenie_id INTEGER  NOT NULL    ,
PRIMARY KEY(id)    ,
  FOREIGN KEY(uzytkownik_id)
    REFERENCES Uzytkownik(id),
  FOREIGN KEY(ogloszenie_id)
    REFERENCES Ogloszenie(id));
GO


CREATE INDEX Ulubiona_FKIndex1 ON Ulubiona (uzytkownik_id);
GO
CREATE INDEX Ulubiona_FKIndex2 ON Ulubiona (ogloszenie_id);
GO


CREATE INDEX IFK_Rel_08 ON Ulubiona (uzytkownik_id);
GO
CREATE INDEX IFK_Rel_09 ON Ulubiona (ogloszenie_id);
GO



