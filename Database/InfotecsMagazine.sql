DROP TABLE IF EXISTS public.comment;
DROP TABLE IF EXISTS public.article;
DROP TABLE IF EXISTS public.user;

CREATE TABLE public.user
(
	id INT GENERATED ALWAYS AS IDENTITY (START WITH 1 INCREMENT BY 1) NOT NULL,
	CONSTRAINT pk_user PRIMARY KEY (id),
	login VARCHAR(320) NOT NULL,
	CONSTRAINT unq_user_login UNIQUE (login),
	password CHAR(64) NOT NULL,
	salt CHAR(24) NOT NULL
);

CREATE TABLE public.article
(
	id INT GENERATED ALWAYS AS IDENTITY (START WITH 1 INCREMENT BY 1) NOT NULL,
	CONSTRAINT pk_article PRIMARY KEY (id),
	title VARCHAR(80) NOT NULL,
	CONSTRAINT unq_article_title UNIQUE (title),
	teaser BYTEA NULL,
	body VARCHAR(60000) NOT NULL,
	CONSTRAINT chk_article_body CHECK (LENGTH(Body) >= 2000),
	userId INT NOT NULL,
	CONSTRAINT fk_article_userId_user_id FOREIGN KEY (userId) REFERENCES public.user (id)
);

CREATE TABLE public.comment
(
	id INT GENERATED ALWAYS AS IDENTITY (START WITH 1 INCREMENT BY 1) NOT NULL,
	CONSTRAINT pk_comment PRIMARY KEY (id),
	body VARCHAR(6000) NOT NULL,
	CONSTRAINT chk_comment_body CHECK (LENGTH(TRIM(Body)) > 0),
	articleId INT NOT NULL,
	CONSTRAINT fk_comment_articleId_article_id FOREIGN KEY (articleId) REFERENCES public.article (id),
	userId INT NOT NULL,
	CONSTRAINT fk_comment_userId_user_Id FOREIGN KEY (userId) REFERENCES public.user (id)
);