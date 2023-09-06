
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 09/06/2023 15:48:08
-- Generated from EDMX file: C:\Users\Paweł Brandt\Documents\GitHub\PBX\PBX\Models\PbxModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [PBXDB];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK__Aplikacja__oglos__41EDCAC5]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Aplikacja] DROP CONSTRAINT [FK__Aplikacja__oglos__41EDCAC5];
GO
IF OBJECT_ID(N'[dbo].[FK__Aplikacja__uzytk__40F9A68C]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Aplikacja] DROP CONSTRAINT [FK__Aplikacja__uzytk__40F9A68C];
GO
IF OBJECT_ID(N'[dbo].[FK__Chat__oglaszajac__4CA06362]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Chat] DROP CONSTRAINT [FK__Chat__oglaszajac__4CA06362];
GO
IF OBJECT_ID(N'[dbo].[FK__Chat__ogloszenie__4BAC3F29]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Chat] DROP CONSTRAINT [FK__Chat__ogloszenie__4BAC3F29];
GO
IF OBJECT_ID(N'[dbo].[FK__Chat__zaintereso__4D94879B]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Chat] DROP CONSTRAINT [FK__Chat__zaintereso__4D94879B];
GO
IF OBJECT_ID(N'[dbo].[FK__Kategoria__nadka__01142BA1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Kategoria] DROP CONSTRAINT [FK__Kategoria__nadka__01142BA1];
GO
IF OBJECT_ID(N'[dbo].[FK__Koszyk__ogloszen__3F115E1A]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Koszyk] DROP CONSTRAINT [FK__Koszyk__ogloszen__3F115E1A];
GO
IF OBJECT_ID(N'[dbo].[FK__Koszyk__uzytkown__3E1D39E1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Koszyk] DROP CONSTRAINT [FK__Koszyk__uzytkown__3E1D39E1];
GO
IF OBJECT_ID(N'[dbo].[FK__Ocena__ocena_dla__3F466844]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Ocena] DROP CONSTRAINT [FK__Ocena__ocena_dla__3F466844];
GO
IF OBJECT_ID(N'[dbo].[FK__Ocena__ocena_od___403A8C7D]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Ocena] DROP CONSTRAINT [FK__Ocena__ocena_od___403A8C7D];
GO
IF OBJECT_ID(N'[dbo].[FK__Ogloszeni__kateg__4316F928]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Ogloszenie] DROP CONSTRAINT [FK__Ogloszeni__kateg__4316F928];
GO
IF OBJECT_ID(N'[dbo].[FK__Ogloszeni__wysta__440B1D61]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Ogloszenie] DROP CONSTRAINT [FK__Ogloszeni__wysta__440B1D61];
GO
IF OBJECT_ID(N'[dbo].[FK__Ulubiona__oglosz__5535A963]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Ulubiona] DROP CONSTRAINT [FK__Ulubiona__oglosz__5535A963];
GO
IF OBJECT_ID(N'[dbo].[FK__Ulubiona__uzytko__5441852A]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Ulubiona] DROP CONSTRAINT [FK__Ulubiona__uzytko__5441852A];
GO
IF OBJECT_ID(N'[dbo].[FK__Wiadomosc__chat___5165187F]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Wiadomosc] DROP CONSTRAINT [FK__Wiadomosc__chat___5165187F];
GO
IF OBJECT_ID(N'[dbo].[FK__Wiadomosc__nadaw__5070F446]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Wiadomosc] DROP CONSTRAINT [FK__Wiadomosc__nadaw__5070F446];
GO
IF OBJECT_ID(N'[dbo].[FK__Zamowieni__oglos__3C34F16F]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Zamowienie] DROP CONSTRAINT [FK__Zamowieni__oglos__3C34F16F];
GO
IF OBJECT_ID(N'[dbo].[FK__Zamowieni__uzytk__3B40CD36]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Zamowienie] DROP CONSTRAINT [FK__Zamowieni__uzytk__3B40CD36];
GO
IF OBJECT_ID(N'[dbo].[FK__Zgloszeni__Oglos__48CFD27E]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Zgloszenie] DROP CONSTRAINT [FK__Zgloszeni__Oglos__48CFD27E];
GO
IF OBJECT_ID(N'[dbo].[FK__Zgloszeni__zglas__46E78A0C]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Zgloszenie] DROP CONSTRAINT [FK__Zgloszeni__zglas__46E78A0C];
GO
IF OBJECT_ID(N'[dbo].[FK__Zgloszeni__zglos__47DBAE45]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Zgloszenie] DROP CONSTRAINT [FK__Zgloszeni__zglos__47DBAE45];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Admin]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Admin];
GO
IF OBJECT_ID(N'[dbo].[Aplikacja]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Aplikacja];
GO
IF OBJECT_ID(N'[dbo].[Chat]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Chat];
GO
IF OBJECT_ID(N'[dbo].[Kategoria]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Kategoria];
GO
IF OBJECT_ID(N'[dbo].[Koszyk]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Koszyk];
GO
IF OBJECT_ID(N'[dbo].[Ocena]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Ocena];
GO
IF OBJECT_ID(N'[dbo].[Ogloszenie]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Ogloszenie];
GO
IF OBJECT_ID(N'[dbo].[Ulubiona]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Ulubiona];
GO
IF OBJECT_ID(N'[dbo].[Usunieci]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Usunieci];
GO
IF OBJECT_ID(N'[dbo].[Uzytkownik]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Uzytkownik];
GO
IF OBJECT_ID(N'[dbo].[Wiadomosc]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Wiadomosc];
GO
IF OBJECT_ID(N'[dbo].[Zamowienie]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Zamowienie];
GO
IF OBJECT_ID(N'[dbo].[Zgloszenie]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Zgloszenie];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Admin'
CREATE TABLE [dbo].[Admin] (
    [id] int IDENTITY(1,1) NOT NULL,
    [login] varchar(100)  NOT NULL,
    [haslo] varchar(100)  NOT NULL
);
GO

-- Creating table 'Chat'
CREATE TABLE [dbo].[Chat] (
    [id] int IDENTITY(1,1) NOT NULL,
    [ogloszenie_id] int  NOT NULL,
    [zainteresowany_id] int  NOT NULL,
    [oglaszajacy_id] int  NOT NULL
);
GO

-- Creating table 'Kategoria'
CREATE TABLE [dbo].[Kategoria] (
    [id] int IDENTITY(1,1) NOT NULL,
    [nadkategoria_id] int  NULL,
    [nazwa] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'Ocena'
CREATE TABLE [dbo].[Ocena] (
    [id] int IDENTITY(1,1) NOT NULL,
    [ocena_od_id] int  NOT NULL,
    [ocena_dla_id] int  NOT NULL,
    [ocena] float  NOT NULL,
    [komentarz] nchar(100)  NULL
);
GO

-- Creating table 'Ogloszenie'
CREATE TABLE [dbo].[Ogloszenie] (
    [id] int IDENTITY(1,1) NOT NULL,
    [wystawil_id] int  NOT NULL,
    [kategoria_id] int  NOT NULL,
    [nazwa] nvarchar(100)  NOT NULL,
    [opis] nvarchar(500)  NOT NULL,
    [aktywne] bit  NOT NULL,
    [dodano] datetime  NOT NULL,
    [typ] nvarchar(50)  NOT NULL,
    [cena] float  NOT NULL,
    [negocjacja] bit  NOT NULL,
    [pokaz_tel] bit  NOT NULL,
    [pokaz_email] bit  NOT NULL,
    [zdjecie] varbinary(max)  NOT NULL,
    [lokalizacja] nvarchar(50)  NOT NULL,
    [auto_przedluzanie] bit  NOT NULL
);
GO

-- Creating table 'Ulubiona'
CREATE TABLE [dbo].[Ulubiona] (
    [id] int IDENTITY(1,1) NOT NULL,
    [uzytkownik_id] int  NOT NULL,
    [ogloszenie_id] int  NOT NULL
);
GO

-- Creating table 'Usunieci'
CREATE TABLE [dbo].[Usunieci] (
    [id] int IDENTITY(1,1) NOT NULL,
    [imie] nvarchar(30)  NOT NULL,
    [nr_tel] varchar(100)  NOT NULL,
    [email] varchar(100)  NOT NULL,
    [haslo] varchar(100)  NOT NULL,
    [dolaczono] datetime  NOT NULL,
    [usunieto] datetime  NOT NULL,
    [zdjecie] varbinary(max)  NULL
);
GO

-- Creating table 'Uzytkownik'
CREATE TABLE [dbo].[Uzytkownik] (
    [id] int IDENTITY(1,1) NOT NULL,
    [imie] nvarchar(30)  NOT NULL,
    [nr_tel] varchar(100)  NOT NULL,
    [email] varchar(100)  NOT NULL,
    [haslo] varchar(100)  NOT NULL,
    [dolaczono] datetime  NOT NULL,
    [zdjecie] varbinary(max)  NOT NULL
);
GO

-- Creating table 'Wiadomosc'
CREATE TABLE [dbo].[Wiadomosc] (
    [id] int IDENTITY(1,1) NOT NULL,
    [chat_id] int  NOT NULL,
    [nadawca_id] int  NOT NULL,
    [wiadomosc] nvarchar(500)  NOT NULL,
    [wyslano] datetime  NOT NULL,
    [przeczytano] bit  NOT NULL
);
GO

-- Creating table 'Zgloszenie'
CREATE TABLE [dbo].[Zgloszenie] (
    [id] int IDENTITY(1,1) NOT NULL,
    [ogloszenie_id] int  NOT NULL,
    [zglaszajacy_id] int  NOT NULL,
    [zgloszony_id] int  NOT NULL,
    [tresc] nvarchar(500)  NOT NULL
);
GO

-- Creating table 'Aplikacja'
CREATE TABLE [dbo].[Aplikacja] (
    [ogloszenie_id] int  NOT NULL,
    [uzytkownik_id] int  NOT NULL,
    [cv] varbinary(max)  NOT NULL,
    [id] int IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'Koszyk'
CREATE TABLE [dbo].[Koszyk] (
    [ogloszenie_id] int  NOT NULL,
    [uzytkownik_id] int  NOT NULL,
    [id] int IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'Zamowienie'
CREATE TABLE [dbo].[Zamowienie] (
    [ogloszenie_id] int  NOT NULL,
    [uzytkownik_id] int  NOT NULL,
    [id] int IDENTITY(1,1) NOT NULL,
    [data] datetime  NOT NULL,
    [status] nvarchar(30)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [id] in table 'Admin'
ALTER TABLE [dbo].[Admin]
ADD CONSTRAINT [PK_Admin]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'Chat'
ALTER TABLE [dbo].[Chat]
ADD CONSTRAINT [PK_Chat]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'Kategoria'
ALTER TABLE [dbo].[Kategoria]
ADD CONSTRAINT [PK_Kategoria]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'Ocena'
ALTER TABLE [dbo].[Ocena]
ADD CONSTRAINT [PK_Ocena]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'Ogloszenie'
ALTER TABLE [dbo].[Ogloszenie]
ADD CONSTRAINT [PK_Ogloszenie]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'Ulubiona'
ALTER TABLE [dbo].[Ulubiona]
ADD CONSTRAINT [PK_Ulubiona]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'Usunieci'
ALTER TABLE [dbo].[Usunieci]
ADD CONSTRAINT [PK_Usunieci]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'Uzytkownik'
ALTER TABLE [dbo].[Uzytkownik]
ADD CONSTRAINT [PK_Uzytkownik]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'Wiadomosc'
ALTER TABLE [dbo].[Wiadomosc]
ADD CONSTRAINT [PK_Wiadomosc]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'Zgloszenie'
ALTER TABLE [dbo].[Zgloszenie]
ADD CONSTRAINT [PK_Zgloszenie]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'Aplikacja'
ALTER TABLE [dbo].[Aplikacja]
ADD CONSTRAINT [PK_Aplikacja]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'Koszyk'
ALTER TABLE [dbo].[Koszyk]
ADD CONSTRAINT [PK_Koszyk]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [id] in table 'Zamowienie'
ALTER TABLE [dbo].[Zamowienie]
ADD CONSTRAINT [PK_Zamowienie]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [oglaszajacy_id] in table 'Chat'
ALTER TABLE [dbo].[Chat]
ADD CONSTRAINT [FK__Chat__oglaszajac__4CA06362]
    FOREIGN KEY ([oglaszajacy_id])
    REFERENCES [dbo].[Uzytkownik]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Chat__oglaszajac__4CA06362'
CREATE INDEX [IX_FK__Chat__oglaszajac__4CA06362]
ON [dbo].[Chat]
    ([oglaszajacy_id]);
GO

-- Creating foreign key on [ogloszenie_id] in table 'Chat'
ALTER TABLE [dbo].[Chat]
ADD CONSTRAINT [FK__Chat__ogloszenie__4BAC3F29]
    FOREIGN KEY ([ogloszenie_id])
    REFERENCES [dbo].[Ogloszenie]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Chat__ogloszenie__4BAC3F29'
CREATE INDEX [IX_FK__Chat__ogloszenie__4BAC3F29]
ON [dbo].[Chat]
    ([ogloszenie_id]);
GO

-- Creating foreign key on [zainteresowany_id] in table 'Chat'
ALTER TABLE [dbo].[Chat]
ADD CONSTRAINT [FK__Chat__zaintereso__4D94879B]
    FOREIGN KEY ([zainteresowany_id])
    REFERENCES [dbo].[Uzytkownik]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Chat__zaintereso__4D94879B'
CREATE INDEX [IX_FK__Chat__zaintereso__4D94879B]
ON [dbo].[Chat]
    ([zainteresowany_id]);
GO

-- Creating foreign key on [chat_id] in table 'Wiadomosc'
ALTER TABLE [dbo].[Wiadomosc]
ADD CONSTRAINT [FK__Wiadomosc__chat___5165187F]
    FOREIGN KEY ([chat_id])
    REFERENCES [dbo].[Chat]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Wiadomosc__chat___5165187F'
CREATE INDEX [IX_FK__Wiadomosc__chat___5165187F]
ON [dbo].[Wiadomosc]
    ([chat_id]);
GO

-- Creating foreign key on [nadkategoria_id] in table 'Kategoria'
ALTER TABLE [dbo].[Kategoria]
ADD CONSTRAINT [FK__Kategoria__nadka__01142BA1]
    FOREIGN KEY ([nadkategoria_id])
    REFERENCES [dbo].[Kategoria]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Kategoria__nadka__01142BA1'
CREATE INDEX [IX_FK__Kategoria__nadka__01142BA1]
ON [dbo].[Kategoria]
    ([nadkategoria_id]);
GO

-- Creating foreign key on [kategoria_id] in table 'Ogloszenie'
ALTER TABLE [dbo].[Ogloszenie]
ADD CONSTRAINT [FK__Ogloszeni__kateg__4316F928]
    FOREIGN KEY ([kategoria_id])
    REFERENCES [dbo].[Kategoria]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Ogloszeni__kateg__4316F928'
CREATE INDEX [IX_FK__Ogloszeni__kateg__4316F928]
ON [dbo].[Ogloszenie]
    ([kategoria_id]);
GO

-- Creating foreign key on [ocena_dla_id] in table 'Ocena'
ALTER TABLE [dbo].[Ocena]
ADD CONSTRAINT [FK__Ocena__ocena_dla__3F466844]
    FOREIGN KEY ([ocena_dla_id])
    REFERENCES [dbo].[Uzytkownik]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Ocena__ocena_dla__3F466844'
CREATE INDEX [IX_FK__Ocena__ocena_dla__3F466844]
ON [dbo].[Ocena]
    ([ocena_dla_id]);
GO

-- Creating foreign key on [ocena_od_id] in table 'Ocena'
ALTER TABLE [dbo].[Ocena]
ADD CONSTRAINT [FK__Ocena__ocena_od___403A8C7D]
    FOREIGN KEY ([ocena_od_id])
    REFERENCES [dbo].[Uzytkownik]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Ocena__ocena_od___403A8C7D'
CREATE INDEX [IX_FK__Ocena__ocena_od___403A8C7D]
ON [dbo].[Ocena]
    ([ocena_od_id]);
GO

-- Creating foreign key on [ogloszenie_id] in table 'Aplikacja'
ALTER TABLE [dbo].[Aplikacja]
ADD CONSTRAINT [FK__Aplikacja__oglos__41EDCAC5]
    FOREIGN KEY ([ogloszenie_id])
    REFERENCES [dbo].[Ogloszenie]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Aplikacja__oglos__41EDCAC5'
CREATE INDEX [IX_FK__Aplikacja__oglos__41EDCAC5]
ON [dbo].[Aplikacja]
    ([ogloszenie_id]);
GO

-- Creating foreign key on [wystawil_id] in table 'Ogloszenie'
ALTER TABLE [dbo].[Ogloszenie]
ADD CONSTRAINT [FK__Ogloszeni__wysta__440B1D61]
    FOREIGN KEY ([wystawil_id])
    REFERENCES [dbo].[Uzytkownik]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Ogloszeni__wysta__440B1D61'
CREATE INDEX [IX_FK__Ogloszeni__wysta__440B1D61]
ON [dbo].[Ogloszenie]
    ([wystawil_id]);
GO

-- Creating foreign key on [ogloszenie_id] in table 'Ulubiona'
ALTER TABLE [dbo].[Ulubiona]
ADD CONSTRAINT [FK__Ulubiona__oglosz__5535A963]
    FOREIGN KEY ([ogloszenie_id])
    REFERENCES [dbo].[Ogloszenie]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Ulubiona__oglosz__5535A963'
CREATE INDEX [IX_FK__Ulubiona__oglosz__5535A963]
ON [dbo].[Ulubiona]
    ([ogloszenie_id]);
GO

-- Creating foreign key on [ogloszenie_id] in table 'Zgloszenie'
ALTER TABLE [dbo].[Zgloszenie]
ADD CONSTRAINT [FK__Zgloszeni__Oglos__48CFD27E]
    FOREIGN KEY ([ogloszenie_id])
    REFERENCES [dbo].[Ogloszenie]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Zgloszeni__Oglos__48CFD27E'
CREATE INDEX [IX_FK__Zgloszeni__Oglos__48CFD27E]
ON [dbo].[Zgloszenie]
    ([ogloszenie_id]);
GO

-- Creating foreign key on [uzytkownik_id] in table 'Ulubiona'
ALTER TABLE [dbo].[Ulubiona]
ADD CONSTRAINT [FK__Ulubiona__uzytko__5441852A]
    FOREIGN KEY ([uzytkownik_id])
    REFERENCES [dbo].[Uzytkownik]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Ulubiona__uzytko__5441852A'
CREATE INDEX [IX_FK__Ulubiona__uzytko__5441852A]
ON [dbo].[Ulubiona]
    ([uzytkownik_id]);
GO

-- Creating foreign key on [uzytkownik_id] in table 'Aplikacja'
ALTER TABLE [dbo].[Aplikacja]
ADD CONSTRAINT [FK__Aplikacja__uzytk__40F9A68C]
    FOREIGN KEY ([uzytkownik_id])
    REFERENCES [dbo].[Uzytkownik]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Aplikacja__uzytk__40F9A68C'
CREATE INDEX [IX_FK__Aplikacja__uzytk__40F9A68C]
ON [dbo].[Aplikacja]
    ([uzytkownik_id]);
GO

-- Creating foreign key on [nadawca_id] in table 'Wiadomosc'
ALTER TABLE [dbo].[Wiadomosc]
ADD CONSTRAINT [FK__Wiadomosc__nadaw__5070F446]
    FOREIGN KEY ([nadawca_id])
    REFERENCES [dbo].[Uzytkownik]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Wiadomosc__nadaw__5070F446'
CREATE INDEX [IX_FK__Wiadomosc__nadaw__5070F446]
ON [dbo].[Wiadomosc]
    ([nadawca_id]);
GO

-- Creating foreign key on [zglaszajacy_id] in table 'Zgloszenie'
ALTER TABLE [dbo].[Zgloszenie]
ADD CONSTRAINT [FK__Zgloszeni__zglas__46E78A0C]
    FOREIGN KEY ([zglaszajacy_id])
    REFERENCES [dbo].[Uzytkownik]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Zgloszeni__zglas__46E78A0C'
CREATE INDEX [IX_FK__Zgloszeni__zglas__46E78A0C]
ON [dbo].[Zgloszenie]
    ([zglaszajacy_id]);
GO

-- Creating foreign key on [zgloszony_id] in table 'Zgloszenie'
ALTER TABLE [dbo].[Zgloszenie]
ADD CONSTRAINT [FK__Zgloszeni__zglos__47DBAE45]
    FOREIGN KEY ([zgloszony_id])
    REFERENCES [dbo].[Uzytkownik]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Zgloszeni__zglos__47DBAE45'
CREATE INDEX [IX_FK__Zgloszeni__zglos__47DBAE45]
ON [dbo].[Zgloszenie]
    ([zgloszony_id]);
GO

-- Creating foreign key on [ogloszenie_id] in table 'Koszyk'
ALTER TABLE [dbo].[Koszyk]
ADD CONSTRAINT [FK__Koszyk__ogloszen__3F115E1A]
    FOREIGN KEY ([ogloszenie_id])
    REFERENCES [dbo].[Ogloszenie]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Koszyk__ogloszen__3F115E1A'
CREATE INDEX [IX_FK__Koszyk__ogloszen__3F115E1A]
ON [dbo].[Koszyk]
    ([ogloszenie_id]);
GO

-- Creating foreign key on [uzytkownik_id] in table 'Koszyk'
ALTER TABLE [dbo].[Koszyk]
ADD CONSTRAINT [FK__Koszyk__uzytkown__3E1D39E1]
    FOREIGN KEY ([uzytkownik_id])
    REFERENCES [dbo].[Uzytkownik]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Koszyk__uzytkown__3E1D39E1'
CREATE INDEX [IX_FK__Koszyk__uzytkown__3E1D39E1]
ON [dbo].[Koszyk]
    ([uzytkownik_id]);
GO

-- Creating foreign key on [ogloszenie_id] in table 'Zamowienie'
ALTER TABLE [dbo].[Zamowienie]
ADD CONSTRAINT [FK__Zamowieni__oglos__3C34F16F]
    FOREIGN KEY ([ogloszenie_id])
    REFERENCES [dbo].[Ogloszenie]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Zamowieni__oglos__3C34F16F'
CREATE INDEX [IX_FK__Zamowieni__oglos__3C34F16F]
ON [dbo].[Zamowienie]
    ([ogloszenie_id]);
GO

-- Creating foreign key on [uzytkownik_id] in table 'Zamowienie'
ALTER TABLE [dbo].[Zamowienie]
ADD CONSTRAINT [FK__Zamowieni__uzytk__3B40CD36]
    FOREIGN KEY ([uzytkownik_id])
    REFERENCES [dbo].[Uzytkownik]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK__Zamowieni__uzytk__3B40CD36'
CREATE INDEX [IX_FK__Zamowieni__uzytk__3B40CD36]
ON [dbo].[Zamowienie]
    ([uzytkownik_id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------