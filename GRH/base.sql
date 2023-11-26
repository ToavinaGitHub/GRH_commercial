CREATE TABLE Services(
   idService INT IDENTITY,
   nomService VARCHAR(255)  NOT NULL,
   email VARCHAR(255) ,
   mdp VARCHAR(50) ,
   PRIMARY KEY(idService),
   UNIQUE(nomService),
   UNIQUE(email)
);

insert into services(nomService, email, mdp) values ('commercial','commercial@gmail.com','commercial');
insert into services (nomService,Email,mdp) values ('Informatique','info@gmail.com','info')
select * from services

/* GRH */
create table Postes(
    idPoste int IDENTITY(1,1) PRIMARY KEY,
    nomPoste VARCHAR(255),
    idService int,
    Foreign Key (idService) REFERENCES Services(idService)
)

insert into Postes(nomPoste,idService) values ('Developeur',1)

create table RH(
    idRH int IDENTITY(1,1) PRIMARY KEY,
    email VARCHAR(255),
    mdp VARCHAR(255)
)
insert into RH(email,mdp) values ("rh@gmail.com","rh123")

CREATE table Critere(
    idCritere int IDENTITY(1,1) PRIMARY KEY,
    nomCritere VARCHAR(255)
)
insert into Critere(nomCritere) values ('diplome'),('experience'),('sexe'),('situation matrimoniale'),('nationalite')

CREATE table SousCritere(
    idSousCritere int IDENTITY(1,1) PRIMARY KEY,
    idCritere int,
    nomSousCritere VARCHAR(255),
    Foreign Key (idCritere) REFERENCES Critere(idCritere)
)
insert into SousCritere(idCritere,nomSousCritere) values (1,'bepc'),(1,'bacc'),(1,'Bacc+3'),(1,'Bacc+4'),(1,'Bacc+5')
insert into SousCritere(idCritere,nomSousCritere) values (2,'1 a 3 ans'),(2,'4 a 6 ans'),(2,'7 ans et plus')
insert into SousCritere(idCritere,nomSousCritere) values (3,'homme'),(3,'femme')
insert into SousCritere(idCritere,nomSousCritere) values (4,'celibataire'),(4,'marie(e)'),(4,'veuf(ve)')
insert into SousCritere(idCritere,nomSousCritere) values (5,'malgache'),(5,'etranger')

create table Annonce (
    idAnnonce int IDENTITY(1,1) PRIMARY key,
    idPoste int,
    descriptions VARCHAR(255),
    volumeTache  int,
    volumeJourHomme int,
    dateAnnonce datetime,
    etat int,
    Foreign Key (idPoste) REFERENCES Postes(idPoste) 
)

insert into annonce (idPoste,descriptions,volumeTache,volumeJourHomme,dateAnnonce,etat) values (1,'Recrutement pour le poste de developpeur Senior Java',8,8,'2023-09-01',0)
insert into annonce (idPoste,descriptions,volumeTache,volumeJourHomme,dateAnnonce,etat) values (1,'Recrutement pour le poste de developpeur Junior',8,8,'2023-09-01',0)

create table critereCoef(
    idCriCoef int IDENTITY(1,1) PRIMARY key,
    idSousCritere int,
    coef int,
    idAnnonce int,
    Foreign Key (idSousCritere) REFERENCES SousCritere(idSousCritere),
    Foreign Key (idAnnonce) REFERENCES Annonce(idAnnonce)
)

insert into critereCoef(idSousCritere,coef,idAnnonce) values (1,0,1),(2,1,1),(3,2,1),(4,3,1),(5,4,1)
insert into critereCoef(idSousCritere,coef,idAnnonce) values (6,1,1),(7,2,1),(8,3,1)
insert into critereCoef(idSousCritere,coef,idAnnonce) values (9,1,1),(10,2,1)
insert into critereCoef(idSousCritere,coef,idAnnonce) values (11,1,1),(12,3,1),(13,2,1)
insert into critereCoef(idSousCritere,coef,idAnnonce) values (14,2,1),(15,1,1)

insert into critereCoef(idSousCritere,coef,idAnnonce) values (1,0,2),(2,1,2),(3,2,2),(4,3,2),(5,4,2)
insert into critereCoef(idSousCritere,coef,idAnnonce) values (6,1,2),(7,2,2),(8,3,2)
insert into critereCoef(idSousCritere,coef,idAnnonce) values (9,1,2),(10,2,2)
insert into critereCoef(idSousCritere,coef,idAnnonce) values (11,1,2),(12,3,2),(13,2,2)
insert into critereCoef(idSousCritere,coef,idAnnonce) values (14,2,2),(15,1,2)

CREATE Table Question(
    idQuestion int IDENTITY(1,1) PRIMARY KEY,
    nom VARCHAR(255),
    coef int,
    idService int,
    Foreign Key (idService) REFERENCES Services(idService)
)

insert into Question(nom,coef,idService) values ("Quel est le role d'un pare-feu (firewall) dans un reseau informatique?",2,1)
insert into Question(nom,coef,idService) values ("Qu'est-ce que le cloud computing?",3,1)
insert into Question(nom,coef,idService) values ("Quelles sont les etapes du processus de developpement de logiciels (SDLC - Software Development Life Cycle)",3,1)
insert into Question(nom,coef,idService) values ("Quel protocole de communication est couramment utilise pour envoyer des courriers electroniques?",2,1)
INSERT INTO Question(nom,coef,idService) VALUES ("Quels sont les types de bases de donnees NoSQL populaires?",2, 1)
INSERT INTO Question(nom,coef,idService) VALUES ("Quelles sont les langues de programmation couramment utilisees pour l'IA et l'apprentissage automatique?",2, 1)
INSERT INTO Question(nom,coef,idService) VALUES ("Quelles sont les principales caracteristiques de Docker?",3, 1)
INSERT INTO Question(nom,coef,idService) VALUES ("Quels sont les principaux avantages de l'architecture de microservices?",3, 1)

CREATE table Reponse(
    idReponse int IDENTITY(1,1) primary KEY,
    idQuestion int,
    nom VARCHAR(255),
    etat int,
    Foreign Key (idQuestion) REFERENCES Question(idQuestion)
)

insert into reponse(idQuestion,nom,etat) values (1,"Filtrer le courrier electronique indesirable",0)
insert into reponse(idQuestion,nom,etat) values (1,"Empecher les acces non autorises et les menaces de securite",1)
insert into reponse(idQuestion,nom,etat) values (1,"Accelerer la vitesse de la connexion Internet",0)

insert into reponse(idQuestion,nom,etat) values (2," Une technologie de stockage en reseau",0)
insert into reponse(idQuestion,nom,etat) values (2,"La fourniture de services informatiques via Internet",1)
insert into reponse(idQuestion,nom,etat) values (2,"Un systeme de sauvegarde automatique",0)

insert into reponse(idQuestion,nom,etat) values (3,"Analyse des besoins",1)
insert into reponse(idQuestion,nom,etat) values (3," Conception",1)
insert into reponse(idQuestion,nom,etat) values (3,"Test",1)
insert into reponse(idQuestion,nom,etat) values (3,"Maintenance",1) 

insert into reponse(idQuestion,nom,etat) values (4,"HTTP",0)
insert into reponse(idQuestion,nom,etat) values (4,"FTP",0)
insert into reponse(idQuestion,nom,etat) values (4,"SMTP",1)
insert into reponse(idQuestion,nom,etat) values (4,"DNS",0)

INSERT INTO Reponse (idQuestion, nom, etat) VALUES (5 , "MongoDB", 1);
INSERT INTO Reponse (idQuestion, nom, etat) VALUES (5, "Cassandra", 1);
INSERT INTO Reponse (idQuestion, nom, etat) VALUES (5, "Redis", 1);
INSERT INTO Reponse (idQuestion, nom, etat) VALUES (5, "MySQL", 0);

INSERT INTO Reponse (idQuestion, nom, etat) VALUES (6, "Python", 1);
INSERT INTO Reponse (idQuestion, nom, etat) VALUES (6, "R", 1);
INSERT INTO Reponse (idQuestion, nom, etat) VALUES (6, "Java", 0);
INSERT INTO Reponse (idQuestion, nom, etat) VALUES (6, "C++", 0);

INSERT INTO Reponse (idQuestion, nom, etat) VALUES (7, "Conteneurisation", 1);
INSERT INTO Reponse (idQuestion, nom, etat) VALUES (7, "Isolation des applications", 1);
INSERT INTO Reponse (idQuestion, nom, etat) VALUES (7, "Orchestration", 1);
INSERT INTO Reponse (idQuestion, nom, etat) VALUES (7, "Langage de programmation", 0);

INSERT INTO Reponse (idQuestion, nom, etat) VALUES (8, "evolutivite", 1);
INSERT INTO Reponse (idQuestion, nom, etat) VALUES (8, "Independance des services", 1);
INSERT INTO Reponse (idQuestion, nom, etat) VALUES (8, "Facilite de deploiement", 1);
INSERT INTO Reponse (idQuestion, nom, etat) VALUES (8, "Monolithique", 0);


create table QuestionAnnonce(
    idQuestionAnnonce int IDENTITY(1,1) PRIMARY KEY ,
    idQuestion int,
    idAnnonce int,
    Foreign Key (idQuestion) REFERENCES Question(idQuestion),
    Foreign Key (idAnnonce) REFERENCES Annonce(idAnnonce)
)

create table Clients(
    idClient int IDENTITY(1,1) PRIMARY KEY,
    nom VARCHAR(255),
    prenom VARCHAR(255),
    dtn DATE,
    email VARCHAR(255),
    mdp VARCHAR(255),
    addresse VARCHAR(255),
    sexe int,
    note int
)
insert into clients(nom, prenom,dtn,email,mdp,addresse,sexe,note) values ("Jean","Rakoto","2001-05-15","Jean@gmail.com","jean123","102 Andoharanofotsy",0,0);
insert into clients(nom, prenom,dtn,email,mdp,addresse,sexe,note) values ("Julie","Rasoa","1998-06-20","Julie@gmail.com","julie","101 Antanimena",1,0);
insert into clients(nom, prenom,dtn,email,mdp,addresse,sexe,note) values ("Rabe","Paul","1980-12-02","Paul@gmail.com","paul","101 Ivato",0,0);
insert into clients(nom, prenom,dtn,email,mdp,addresse,sexe,note) values ("Rak","Jeanne","1995-04-07","Jeanne@gmail.com","jeanne","101 Alasora",1,0);


create table cv(
    idCv int IDENTITY(1,1) PRIMARY KEY,
    idAnnonce int,
    idClient int,
    pdfDiplome VARCHAR(255),
    pdfAttestation VARCHAR(255),
    Foreign Key (idClient) REFERENCES Clients(idClient),
    foreign key (idAnnonce) references Annonce(idAnnonce)
)

insert into cv(idAnnonce,idClient,pdfDiplome,pdfAttestation) values (1,1,'pdf diplome','pdf Attestation'),(1,2,'pdf diplome','pdf Attestation'),(1,3,'pdf diplome','pdf Attestation'),(1,4,'pdf diplome','pdf Attestation')

create table DetailsCv(
    idCv int,
    idSousCritere int,
    Foreign Key (idSousCritere) REFERENCES SousCritere(idSousCritere),
    foreign key (idCv) references cv(idCv)
)

insert into DetailsCv(idCv,idSousCritere) values (1,3),(1,6),(1,11),(1,14)
insert into DetailsCv(idCv,idSousCritere) values (2,4),(2,7),(2,11),(2,14)
insert into DetailsCv(idCv,idSousCritere) values (3,5),(3,6),(3,13),(3,14)
insert into DetailsCv(idCv,idSousCritere) values (4,3),(4,8),(4,12),(4,14)

CREATE table etatClient (
    idEtatClient int IDENTITY(1,1) PRIMARY KEY,
    idClient int,
    etat int,
    idAnnonce int,
    dateDepot datetime,
    dateTeste datetime,
    dateEntretien datetime,
    Foreign Key (idClient) REFERENCES Clients(idClient),
    Foreign Key (idAnnonce) REFERENCES Annonce(idAnnonce) 
)                

insert into etatClient (idClient,etat,idAnnonce,dateDepot,dateTeste,dateEntretien) values (1,0,1,"2023-09-05 08:00:00",null,null)
insert into etatClient (idClient,etat,idAnnonce,dateDepot,dateTeste,dateEntretien) values (2,0,1,"2023-09-08 15:10:20",null,null)
insert into etatClient (idClient,etat,idAnnonce,dateDepot,dateTeste,dateEntretien) values (3,0,1,"2023-09-10 08:00:00",null,null)
insert into etatClient (idClient,etat,idAnnonce,dateDepot,dateTeste,dateEntretien) values (4,0,1,"2023-09-12 08:00:00",null,null)

create table note(
    idCv int,
    noteCV int,
    noteTest int,
    noteEntretien int,
    foreign key(idCv) references cv(idcv)
)

create view v_clientAnnonce as
    select idCv,idAnnonce,c.idClient,nom,prenom,dtn,email,mdp,addresse,sexe,note from cv join clients c on cv.idClient=c.idClient

create view v_detailcv as 
    select cv.idAnnonce,dc.idcv,dc.idSousCritere,nomSousCritere,c.idCritere,nomCritere from detailscv dc join souscritere sc on dc.idSouscritere=sc.idSouscritere join critere c on sc.idCritere=c.idCritere join cv on dc.idcv=cv.idcv

create view v_notesexe as
    select coef,nomSousCritere,idAnnonce from criterecoef cc join souscritere sc on cc.idSousCritere=sc.idSousCritere

create view v_servicePoste as
    select s.idService,nomService,email,mdp,idPoste from services s join postes p on s.idservice=p.idservice

create view v_posteAnnonce as
    select p.idPoste,nomPoste,idAnnonce from postes p join annonce a on p.idPoste=a.idPoste


/* /// COMMERCIAL /// */

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

insert into bonDeCommande(titre,daty,jourLivraison,conditionPayement,etat,idTypePayement,idFournisseur) VALUES ('Bon de Commande','2023-19-12 14:30:00',5,'paiement dû dans 30 jours',0,4,1);
INSERT INTO bonDeCommande (titre, daty, jourLivraison, conditionPayement, etat, idTypePayement, idFournisseur)
VALUES ('Bon de Commande', '2023-19-12 14:30:00', 5, 'paiement dû dans 30 jours', 0, 4, 1);

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
Les coups de lame de forme régulière sont bien jetés, et la grigne ambrée est bien gonflée.',1,1);
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
insert into proforma(prixUnitaire, tva, daty, idArticle, idFournisseur) values (1000,200,'2023-21-10 14:00:00',1,1)
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
insert into detailsBonCommande(quantite,idProforma,idBonDeCommande) values (10,1,2)
select * from detailsBonCommande

CREATE TABLE ClientVente(
   idClientVente INT IDENTITY primary key,
   nomClientVente VARCHAR(255),
   email VARCHAR(255),
   mdp VARCHAR(255)
)

insert into ClientVente(nomClientVente,email,mdp) values ('Rasoa Jeanne','jeanne@gmail.com','jeanne')
insert into ClientVente(nomClientVente,email,mdp) values ('Rabe Jean','jean@gmail.com','jean')

CREATE TABLE ArticleVente(
   idArticleVente INT IDENTITY primary key,
   nomArticleVente VARCHAR(255)
)

insert into ArticleVente(nomArticleVente) values ('Stylos'),('Lait'),('Riz')

CREATE TABLE UniteArticle(
   idUniteArticle INT IDENTITY primary key,
   idArticleVente int,
   nomUnite VARCHAR(255),
   quantite double precision,
   FOREIGN KEY(idArticleVente) references ArticleVente(idArticleVente)
)

insert into UniteArticle(idArticleVente,nomUnite,quantite) values (1,'pieces',1),(2,'litre',1),(3,'kg',1)

CREATE TABLE ProformaVente(
   idProformaVente INT IDENTITY primary key,
   idArticleVente int,
   quantite double precision,
   dateProformaVente datetime,
   etat int,
   idUniteArticle int,
   idClientVente int,
   foreign key(idArticleVente) references ArticleVente(idArticleVente),
   foreign key(idUniteArticle) references UniteArticle(idUniteArticle),
   foreign key(idClientVente) references ClientVente(idClientVente)
)

insert into ProformaVente(idArticleVente,quantite,dateProformaVente,etat,idUniteArticle,idClientVente) values (1,10,'2023-26-11 08:00:00',0,1,1)
insert into ProformaVente(idArticleVente,quantite,dateProformaVente,etat,idUniteArticle,idClientVente) values (1,60,'2023-28-11 08:00:00',0,1,1)

CREATE TABLE EntreeStock(
   idEntreeStock int IDENTITY primary key,
   idArticleVente int,
   pu double precision,
   dateEntree datetime,
   quantite double precision,
   reste double precision,
   foreign key(idArticleVente) references ArticleVente(idArticleVente)
)

insert into EntreeStock(idArticleVente,pu,dateEntree,quantite,reste) values (1,500,'2023-11-01 08:00:00',50,50)

create table PrixArticle(
   idPrixArticle int IDENTITY primary key,
   idArticleVente int,
   prix double precision,
   foreign key(idArticleVente) references ArticleVente(idArticleVente)
)

insert into PrixArticle(idArticleVente,prix) values (1,700)
insert into PrixArticle(idArticleVente,prix) values (2,4800)
