DROP TABLE IF EXISTS keliones_marsruto_dalys;
DROP TABLE IF EXISTS keliones;
DROP TABLE IF EXISTS krepseliai;
DROP TABLE IF EXISTS vertimai;
DROP TABLE IF EXISTS uzsakymai;
DROP TABLE IF EXISTS atsiliepimai;
DROP TABLE IF EXISTS korteles;
DROP TABLE IF EXISTS vartotojai;
DROP TABLE IF EXISTS marsruto_stoteles;
DROP TABLE IF EXISTS stoteles;
DROP TABLE IF EXISTS uzsakymo_busenos;
DROP TABLE IF EXISTS marsrutai;
DROP TABLE IF EXISTS roles;
DROP TABLE IF EXISTS korteles_tipai;
CREATE TABLE korteles_tipai
(
	id int,
	name text NOT NULL,
	PRIMARY KEY(id)
);
INSERT INTO korteles_tipai(id, name) VALUES(1, 'Mastercard');
INSERT INTO korteles_tipai(id, name) VALUES(2, 'Visa');

CREATE TABLE roles
(
	id int,
	name text NOT NULL,
	PRIMARY KEY(id)
);
INSERT INTO roles(id, name) VALUES(1, 'Klientas');
INSERT INTO roles(id, name) VALUES(2, 'Kelionu organizatorius');
INSERT INTO roles(id, name) VALUES(3, 'Administratorius');

CREATE TABLE marsrutai
(
	nr int NOT NULL,
	pavadinimas varchar (255) NOT NULL,
	tarifas double precision NOT NULL,
	PRIMARY KEY(nr)
);

CREATE TABLE uzsakymo_busenos
(
	id int,
	name text NOT NULL,
	PRIMARY KEY(id)
);
INSERT INTO uzsakymo_busenos(id, name) VALUES(1, 'Pateiktas');
INSERT INTO uzsakymo_busenos(id, name) VALUES(2, 'Patvirtintas');
INSERT INTO uzsakymo_busenos(id, name) VALUES(3, 'Apmoketas');
INSERT INTO uzsakymo_busenos(id, name) VALUES(4, 'Ivykdytas');
INSERT INTO uzsakymo_busenos(id, name) VALUES(5, 'Atsauktas');

CREATE TABLE stoteles
(
	pavadinimas varchar (255) NOT NULL,
	vieta_pl double precision NOT NULL,
	ivertinimas double NOT NULL,
	vieta_dn double precision NOT NULL,
	id int NOT NULL,
	PRIMARY KEY(id)
);

CREATE TABLE marsruto_stoteles
(
	eil_nr int NOT NULL,
	atstumas double precision NOT NULL,
	laikas int NOT NULL,
	fk_Stotele_id int NOT NULL,
	fk_Marsrutas_nr int NOT NULL,
	PRIMARY KEY(eil_nr, fk_Marsrutas_nr),
	CONSTRAINT turi FOREIGN KEY(fk_Stotele_id) REFERENCES stoteles (id),
	CONSTRAINT sudaro FOREIGN KEY(fk_Marsrutas_nr) REFERENCES marsrutai (nr)
);

CREATE TABLE vartotojai
(
	prof_nuotrauka varchar (255) NOT NULL,
	slaptazodis varchar (255) NOT NULL,
	el_pastas varchar (255) NOT NULL,
	registracijos_data date NOT NULL,
	vardas varchar (255) NOT NULL,
	pavarde varchar (255) NOT NULL,
	gimimo_data date NOT NULL,
	role int NOT NULL,
	id int NOT NULL,
	PRIMARY KEY(id),
	FOREIGN KEY(role) REFERENCES roles (id)
);

CREATE TABLE korteles
(
	numeris int NOT NULL,
	cvv int NOT NULL,
	galiojimo_data date NOT NULL,
	savininko_vardas varchar (255) NOT NULL,
	savininko_pavarde varchar (255) NOT NULL,
	bankas varchar (255) NOT NULL,
	tipas NOT NULL,
	id int NOT NULL,
	fk_Vartotojas_id int NOT NULL,
	PRIMARY KEY(id),
	UNIQUE(fk_Vartotojas_id),
	FOREIGN KEY(tipas) REFERENCES korteles_tipai (id),
	CONSTRAINT turi FOREIGN KEY(fk_Vartotojas_id) REFERENCES vartotojai (id)
);

CREATE TABLE atsiliepimai
(
	data date NOT NULL,
	ivertinimas smallint NOT NULL,
	tekstas varchar (255) NOT NULL,
	id int NOT NULL,
	fk_Vartotojas_id int NOT NULL,
	fk_Stotele_id int NOT NULL,
	PRIMARY KEY(id),
	CONSTRAINT pateikia FOREIGN KEY(fk_Vartotojas_id) REFERENCES vartotojai (id),
	CONSTRAINT turi FOREIGN KEY(fk_Stotele_id) REFERENCES stoteles (id)
);

CREATE TABLE uzsakymai
(
	sukurimo_data date NOT NULL,
	kaina double precision NOT NULL,
	busena NOT NULL,
	id int NOT NULL,
	fk_Kortele_id int NOT NULL,
	fk_Vartotojas_id int NOT NULL,
	PRIMARY KEY(id),
	FOREIGN KEY(busena) REFERENCES uzsakymo_busenos (id),
	CONSTRAINT naudoja FOREIGN KEY(fk_Kortele_id) REFERENCES korteles (id),
	CONSTRAINT formuoja FOREIGN KEY(fk_Vartotojas_id) REFERENCES vartotojai (id)
);

CREATE TABLE vertimai
(
	kalbos_kodas varchar (255) NOT NULL,
	tekstas varchar (255) NOT NULL,
	id int NOT NULL,
	fk_Atsiliepimas_id int NOT NULL,
	PRIMARY KEY(id),
	CONSTRAINT turi FOREIGN KEY(fk_Atsiliepimas_id) REFERENCES atsiliepimai (id)
);

CREATE TABLE krepseliai
(
	sukurimo_data date NOT NULL,
	galutine_kaina double precision NOT NULL,
	id int NOT NULL,
	fk_Uzsakymas_id int NOT NULL,
	fk_Vartotojas_id int NOT NULL,
	PRIMARY KEY(id),
	UNIQUE(fk_Uzsakymas_id),
	CONSTRAINT turi FOREIGN KEY(fk_Uzsakymas_id) REFERENCES uzsakymai (id),
	CONSTRAINT valdo FOREIGN KEY(fk_Vartotojas_id) REFERENCES vartotojai (id)
);

CREATE TABLE keliones
(
	atstumas double precision NOT NULL,
	kaina double precision NOT NULL,
	data_nuo date NOT NULL,
	data_iki date NOT NULL,
	trukme double precision NOT NULL,
	ivertinimas double precision NOT NULL,
	kiekis int NOT NULL,
	id int NOT NULL,
	fk_Krepselis_id int NULL,
	fk_Vartotojas_id int NOT NULL,
	PRIMARY KEY(id),
	CONSTRAINT pridedama FOREIGN KEY(fk_Krepselis_id) REFERENCES krepseliai (id),
	CONSTRAINT planuoja FOREIGN KEY(fk_Vartotojas_id) REFERENCES vartotojai (id)
);

CREATE TABLE keliones_marsruto_dalys
(
	eil_nr int NOT NULL,
	fk_Marsruto_stotele_ NOT NULL,
	fk_Marsruto_stotele_eil_nr int NOT NULL,
	fk_Marsruto_stotele_1 NOT NULL,
	fk_Marsruto_stotele_eil_nr1 int NOT NULL,
	fk_Kelione_id int NOT NULL,
	PRIMARY KEY(eil_nr, fk_Kelione_id),
	CONSTRAINT baigiasi FOREIGN KEY(fk_Marsruto_stotele_, fk_Marsruto_stotele_eil_nr) REFERENCES marsruto_stoteles (eil_nr, fk_Marsrutas_nr),
	CONSTRAINT prasideda FOREIGN KEY(fk_Marsruto_stotele_1, fk_Marsruto_stotele_eil_nr1) REFERENCES marsruto_stoteles (eil_nr, fk_Marsrutas_nr),
	CONSTRAINT sudaryta_is FOREIGN KEY(fk_Kelione_id) REFERENCES keliones (id)
);
