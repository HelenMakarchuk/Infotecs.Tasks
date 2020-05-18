using Microsoft.EntityFrameworkCore.Migrations;

namespace Infotecs.Magazine.Infrastracture.DB.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
				DROP TABLE IF EXISTS public.comment;
                DROP TABLE IF EXISTS public.article;
                DROP TABLE IF EXISTS public.account;				

                CREATE TABLE IF NOT EXISTS public.account
                (
                    id INT GENERATED ALWAYS AS IDENTITY (START WITH 1 INCREMENT BY 1) NOT NULL,
                    CONSTRAINT pk_account PRIMARY KEY (id),
                    login VARCHAR(320) NOT NULL,
                    CONSTRAINT unq_account_login UNIQUE (login),
                    password CHAR(64) NOT NULL,
                    salt CHAR(24) NOT NULL
                );

                CREATE TABLE IF NOT EXISTS public.article
                (
                    id INT GENERATED ALWAYS AS IDENTITY (START WITH 1 INCREMENT BY 1) NOT NULL,
                    CONSTRAINT pk_article PRIMARY KEY (id),
                    title VARCHAR(80) NOT NULL,
                    CONSTRAINT unq_article_title UNIQUE (title),
                    teaser BYTEA NULL,
                    body VARCHAR(60000) NOT NULL,
                    CONSTRAINT chk_article_body CHECK (LENGTH(Body) >= 2000),
                    accountId INT NOT NULL,
                    CONSTRAINT fk_article_accountid_account_id FOREIGN KEY (accountId) REFERENCES public.account (id)
                );

                CREATE TABLE IF NOT EXISTS public.comment
                (
                    id INT GENERATED ALWAYS AS IDENTITY (START WITH 1 INCREMENT BY 1) NOT NULL,
                    CONSTRAINT pk_comment PRIMARY KEY (id),
                    body VARCHAR(6000) NOT NULL,
                    CONSTRAINT chk_comment_body CHECK (LENGTH(TRIM(Body)) > 0),
                    articleid INT NOT NULL,
                    CONSTRAINT fk_comment_articleid_article_id FOREIGN KEY (articleid) REFERENCES public.article (id),
                    accountid INT NOT NULL,
                    CONSTRAINT fk_comment_accountid_account_id FOREIGN KEY (accountid) REFERENCES public.account (id)
                );

                DO $$
                    DECLARE accountId integer := NULL;
                    begin
	                    INSERT INTO account (login, password, salt) 
	                    VALUES (
		                    'admin',
		                    '0000000000000000000000000000000000000000000000000000000000000000',
		                    '000000000000000000000000'
	                    ) 
	                    RETURNING id INTO accountId;

	                    insert into article (title, accountid, body) 
	                    values (
		                    'Lorem ipsum', 
		                    accountId, 
		                    'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, '
                            'quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum '
                            'dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.Lorem ipsum '
                            'dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation '
                            'ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. '
                            'Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.Lorem ipsum dolor sit amet, consectetur adipiscing elit, '
                            'sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. '
                            'Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui '
                            'officia deserunt mollit anim id est laborum.Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. '
                            'Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse '
                            'cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.Excepteur sint occaecat '
                            'cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt '
                            'ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in '
                            'reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim '
                            'id est laborum.Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud '
                            'exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. '
                            'Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.'
	                    );
                END $$;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DROP TABLE IF EXISTS public.comment;
                DROP TABLE IF EXISTS public.article;
                DROP TABLE IF EXISTS public.account;
            ");
        }
    }
}
