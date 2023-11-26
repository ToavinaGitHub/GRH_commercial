use ServiceCo
CREATE TABLE Services(
   idService INT IDENTITY,
   nomService VARCHAR(255)  NOT NULL,
   email VARCHAR(255) ,
   mdp VARCHAR(50) ,
   PRIMARY KEY(idService),
   UNIQUE(nomService),
   UNIQUE(email)
);

insert into services(nomService, email, mdp) values (5,'test','test@gmail.com','test');
select * from services

CREATE TABLE Fournisseur(
   idFournisseur INT IDENTITY,
   nomFournisseur VARCHAR(255) ,
   adresse VARCHAR(255) ,
   email VARCHAR(255) ,
   phone VARCHAR(50)  NOT NULL,
   responsable VARCHAR(255) ,
   PRIMARY KEY(idFournisseur)
);

insert into fournisseur(nomFournisseur,adresse,email,phone,responsable) values ('LEADER PRICE','Akorondrano','leader@gmail.com',0346050214,'RAKOTO');
select * from Fournisseur

CREATE TABLE typePayment(
   idTypePayement INT IDENTITY,
   nomTypePayement VARCHAR(255) ,
   PRIMARY KEY(idTypePayement)
);

insert into typePayment(nomTypePayement) VALUES ('Cheques'), ('especes'),('mobile Money'),('carte Banquaire');
select * from typePayment
CREATE TABLE bonDeCommande(
   idBonDeCommande INT IDENTITY,
   titre VARCHAR(255) ,
   daty DATETIME,
   jourLivraison INT,
   conditionPayement VARCHAR(255) ,
   etat INT,
   idTypePayement INT NOT NULL,
   idFournisseur INT NOT NULL,
   PRIMARY KEY(idBonDeCommande),
   FOREIGN KEY(idTypePayement) REFERENCES typePayment(idTypePayement),
   FOREIGN KEY(idFournisseur) REFERENCES fournisseur(idFournisseur)
);

insert into bonDeCommande(titre,daty,jourLivraison,conditionPayement,etat,idTypePayement,idFournisseur) VALUES ('Bon de Commande','2023-12-19 14:30:00',5,'paiement dû dans 30 jours',0,4,1);
INSERT INTO bonDeCommande (titre, daty, jourLivraison, conditionPayement, etat, idTypePayement, idFournisseur)
VALUES ('Bon de Commande', '2023-12-19T14:30:00', 5, 'paiement dû dans 30 jours', 0, 4, 1);

select * from bonDeCommande

CREATE TABLE unite(
   idUnite INT,
   nomUnite VARCHAR(50) ,
   PRIMARY KEY(idUnite)
);
insert into unite(idUnite,nomUnite) values (1,'pieces');
CREATE TABLE article(
   idArticle INT IDENTITY,
   reference VARCHAR(50) ,
   nomArticle VARCHAR(255) ,
   idUnite INT NOT NULL,
   PRIMARY KEY(idArticle),
   UNIQUE(reference),
   FOREIGN KEY(idUnite) REFERENCES unite(idUnite)
);
insert into article(reference,nomArticle,idUnite) values ('P01','pains',1);

CREATE TABLE besoinArticle(
   idBesoinArticle INT IDENTITY,
   quantite double precision ,
   etat INT, --0 par defaut , 5 raha valider
   descri VARCHAR(255),
   idArticle INT NOT NULL,
   idService INT NOT NULL,
   PRIMARY KEY(idBesoinArticle),
   FOREIGN KEY(idArticle) REFERENCES article(idArticle),
   FOREIGN KEY(idService) REFERENCES Services(idService)
);
insert into besoinArticle(quantite,etat,descri,idArticle,idService) values (10,0,'Pain classique bien développée. La croûte fine, 
craquante et croustillante est de couleur caramel-clair à blonde. 
Les coups de lame de forme régulière sont bien jetés, et la grigne ambrée est bien gonflée.',1,5);
select * from besoinArticle
CREATE TABLE proforma(
   idProforma INT IDENTITY,
   prixUnitaire double precision,
   tva double precision,
   daty DATETIME,
   idArticle INT NOT NULL,
   idFournisseur INT NOT NULL,
   PRIMARY KEY(idProforma),
   FOREIGN KEY(idArticle) REFERENCES article(idArticle),
   FOREIGN KEY(idFournisseur) REFERENCES Fournisseur(idFournisseur)
);
insert into proforma(prixUnitaire, tva, daty, idArticle, idFournisseur) values (1000,200,'2023-10-21T14:00:00',1,1)
select * from proforma
select * from Fournisseur
select * from bonDeCommande
CREATE TABLE detailsBonCommande(
   idDetailsB INT IDENTITY,
   quantite double precision,
   idProforma INT NOT NULL,
   idBonDeCommande INT NOT NULL,
   PRIMARY KEY(idDetailsB),
   FOREIGN KEY(idProforma) REFERENCES proforma(idProforma),
   FOREIGN KEY(idBonDeCommande) REFERENCES bonDeCommande(idBonDeCommande)
);
insert into detailsBonCommande(quantite,idProforma,idBonDeCommande) values (10,2,4)
select * from detailsBonCommande


-------------------------------------------------VENTE----------------------
create table ArticleVente(
   idArticleVente int identity,
   nomArticle VARCHAR(255),
   PRIMARY KEY(idArticle)
);
Create TABLE clientVente(
   idClientVente int identity PRIMARY KEY,
   nomClient VARCHAR(255),
   email VARCHAR(255),
   mdp VARCHAR(255)
);
INSERT into clientVente(nomClient,email,mdp) values ('Client1','client1@gmail.com','client123');
INSERT INTO ArticleVente (nomArticle) VALUES
  ('Spaghetti'),
  ('Riz basmati'),
  ('Céréales de petit déjeuner'),
  ('Café moulu'),
  ('Thé vert en sachets'),
  ('Boîte de conserve de tomates'),
  ('Huile dolive extra vierge'),
  ('Sucre en poudre'),
  ('Boite de thon en conserve'),
  ('Barre de chocolat');

  create table uniteArticle(
   idUniteArticle int PRIMARY KEY,
   idArticleVente int,
   nomUnite VARCHAR(255),
   quantite double precision,
   Foreign Key (idArticleVente) REFERENCES articleVente(idArticleVente)
  );
  insert into uniteArticle(idUniteArticle,idArticleVente,nomUnite,quantite) values (1,1,'cartons',10),(2,1,'unite',1),(3,8,'kg',1)
  ,(4,8,'tonnes',1000),
  (5,8,'sac',70),
  (6,6,'boite',1),
  (7,6,'carton',20),
  (8,5,'sachets',1),
  (9,5,'carton',100),
  (10,2,'kg',1),
  (11,2,'tonnes',1000),
  (12,2,'sacs',80);
create Table proformaVente(
   idProformaVente int identity PRIMARY KEY,
   idArticleVente int,
   quantite double precision,
   daty datetime,
   etat int,
   idUniteArticle int,
   idClientVente int,
   Foreign Key (idClientVente) REFERENCES ClientVente(idClientVente),
   Foreign Key (idUniteArticle) REFERENCES uniteArticle(idUniteArticle),
   Foreign Key (idArticleVente) REFERENCES ArticleVente(idArticleVente)
);
