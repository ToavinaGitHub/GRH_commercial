use ServiceCo
Create TABLE clientVente(
   idClientVente int identity PRIMARY KEY,
   nomClient VARCHAR(255),
   email VARCHAR(255),
   mdp VARCHAR(255)
);
INSERT into clientVente(nomClient,email,mdp) values ('Client1','client1@gmail.com','client123');

select * from article;
insert into article(reference,nomArticle,idUnite) values ('R1','Riz',1);

create table ArticleVente(
   idArticleVente int identity,
   nomArticle VARCHAR(255),
   PRIMARY KEY(idArticleVente)
);

INSERT INTO ArticleVente (nomArticle) VALUES
  ('Pâtes alimentaires '),
  ('Riz basmati'),
  ('Céréales de petit déjeuner'),
  ('Café moulu'),
  ('Thé vert en sachets'),
  ('Boîte de conserve de tomates'),
  ('Huile olive'),
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
  select * from articleVente
  select * from uniteArticle
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

select * from proformaVente