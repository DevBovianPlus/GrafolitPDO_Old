select * from Narocilo_PDO order by 1 desc
select * from Povprasevanje
select * from NarociloPozicija_PDO order by 1 desc
select * from PovprasevanjePozicijaArtikel order by 1 desc
select * from StatusPovprasevanja
	

-- 21.12.2019 - PDO - Polja za pantheon klice in preverjanja
alter table Narocilo_PDO add P_CreateOrder datetime, P_UnsuccCountCreatePDFPantheon int, P_LastTSCreatePDFPantheon datetime, P_TransportOrderPDFName varchar(50), P_TransportOrderPDFDocPath varchar(400), P_GetPDFOrderFile datetime, P_SendWarningToAmin int;
INSERT INTO StatusPovprasevanja values('USTVARJENO_NAROCILO', 'Ustvarjeno naroèilo v Pantheon','Naroèilo je bilo uspešno ustvarjeno v Pantheon-u', 1,CURRENT_TIMESTAMP,CURRENT_TIMESTAMP,1); 
INSERT INTO StatusPovprasevanja values('KREIRAN_PDF', 'Pantheon je kreiral PDF','Pantheon je kreiral PDF,ki se ga pošlje prevozniku', 1,CURRENT_TIMESTAMP,CURRENT_TIMESTAMP,1); 
alter table Narocilo_PDO add StatusID int;
alter table NarociloPozicija_PDO add Ident varchar(50);
ALTER TABLE Narocilo_PDO  WITH CHECK ADD  CONSTRAINT FK_NarociloStatusID FOREIGN KEY(StatusID)REFERENCES [dbo].[StatusPovprasevanja] ([StatusPovprasevanjaID]);

alter table Narocilo_PDO add DobaviteljID int;
ALTER TABLE Narocilo_PDO  WITH CHECK ADD  CONSTRAINT FK_DobaviteljNarociloID FOREIGN KEY(DobaviteljID)REFERENCES [dbo].[Stranka_PDO] ([StrankaID]);

select * from PovprasevanjePozicijaArtikel order by 1 desc

alter table PovprasevanjePozicijaArtikel add Poreklo varchar(10);
alter table NarociloPozicija_PDO add Poreklo varchar(10);
