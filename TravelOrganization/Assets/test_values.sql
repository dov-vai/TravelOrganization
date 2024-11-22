-- Insert one entry into korteles_tipai
INSERT INTO korteles_tipai (id, name)
VALUES (1, 'Mastercard');

-- Insert one entry into roles
INSERT INTO roles (id, name)
VALUES (1, 'Klientas');

-- Insert one entry into marsrutai
INSERT INTO marsrutai (nr, pavadinimas, tarifas)
VALUES (101, 'City Tour', 25.50);

-- Insert one entry into uzsakymo_busenos
INSERT INTO uzsakymo_busenos (id, name)
VALUES (1, 'Pateiktas');

-- Insert one entry into stoteles
INSERT INTO stoteles (id, pavadinimas, vieta_pl, ivertinimas, vieta_dn)
VALUES (1, 'Central Station', 54.6891, 4.5, 25.2798);

-- Insert one entry into marsruto_stoteles
INSERT INTO marsruto_stoteles (eil_nr, atstumas, laikas, fk_Stotele_id, fk_Marsrutas_nr)
VALUES (1, 10.5, 15, 1, 101);

-- Insert one entry into vartotojai
INSERT INTO vartotojai (id, prof_nuotrauka, slaptazodis, el_pastas, registracijos_data, vardas, pavarde, gimimo_data, role)
VALUES (1, 'profile1.jpg', 'password123', 'user@example.com', '2024-11-22', 'John', 'Doe', '1990-01-01', 1);

-- Insert one entry into korteles
INSERT INTO korteles (id, numeris, cvv, galiojimo_data, savininko_vardas, savininko_pavarde, bankas, tipas, fk_Vartotojas_id)
VALUES (1, 1234567812345678, 123, '2026-12-31', 'John', 'Doe', 'BankXYZ', 1, 1);

-- Insert one entry into atsiliepimai
INSERT INTO atsiliepimai (id, data, ivertinimas, tekstas, fk_Vartotojas_id, fk_Stotele_id)
VALUES (1, '2024-11-22', 5, 'Great station!', 1, 1);

-- Insert one entry into vertimai
INSERT INTO vertimai (id, kalbos_kodas, tekstas, fk_Atsiliepimas_id)
VALUES (1, 'en', 'Great station!', 1);

-- Insert one entry into uzsakymai
INSERT INTO uzsakymai (id, sukurimo_data, kaina, busena, fk_Kortele_id, fk_Vartotojas_id)
VALUES (1, '2024-11-22', 50.00, 1, 1, 1);

-- Insert one entry into krepseliai
INSERT INTO krepseliai (id, sukurimo_data, galutine_kaina, fk_Uzsakymas_id, fk_Vartotojas_id)
VALUES (1, '2024-11-22', 50.00, 1, 1);

-- Insert one entry into keliones
INSERT INTO keliones (id, atstumas, kaina, data_nuo, data_iki, trukme, ivertinimas, kiekis, fk_Krepselis_id, fk_Vartotojas_id)
VALUES (1, 100.0, 100.00, '2024-12-01', '2024-12-07', 7.0, 4.5, 10, 1, 1);

-- Insert one entry into keliones_marsruto_dalys
INSERT INTO keliones_marsruto_dalys (eil_nr, fk_Marsruto_stotele_, fk_Marsruto_stotele_eil_nr, fk_Marsruto_stotele_1, fk_Marsruto_stotele_eil_nr1, fk_Kelione_id)
VALUES (1, 1, 1, 1, 1, 1);
