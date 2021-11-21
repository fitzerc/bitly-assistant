/* Database */
CREATE DATABASE bitly
    WITH 
    OWNER = postgres
    ENCODING = 'UTF8'
    LC_COLLATE = 'English_United States.1252'
    LC_CTYPE = 'English_United States.1252'
    TABLESPACE = pg_default
    CONNECTION LIMIT = -1;

/* Tables */
/* bitly_request */
CREATE TABLE IF NOT EXISTS public.bitly_request
(
    request_id integer NOT NULL DEFAULT nextval('bitly_request_request_id_seq'::regclass),
    request_json text COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT bitly_request_pkey PRIMARY KEY (request_id)
)

TABLESPACE pg_default;

ALTER TABLE public.bitly_request
    OWNER to postgres;

/* bitly_response */
CREATE TABLE IF NOT EXISTS public.bitly_response
(
    response_id integer NOT NULL DEFAULT nextval('bitly_response_response_id_seq'::regclass),
    response_json text COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT bitly_response_pkey PRIMARY KEY (response_id)
)

TABLESPACE pg_default;

ALTER TABLE public.bitly_response
    OWNER to postgres;

/* short_link */
CREATE TABLE IF NOT EXISTS public.short_link
(
    short_link_id integer NOT NULL DEFAULT nextval('short_link_short_link_id_seq'::regclass),
    short_link text COLLATE pg_catalog."default" NOT NULL,
    long_link text COLLATE pg_catalog."default" NOT NULL,
    description text COLLATE pg_catalog."default" NOT NULL,
    request_id integer NOT NULL,
    response_id integer NOT NULL,
    date_added date NOT NULL,
    CONSTRAINT short_link_pkey PRIMARY KEY (short_link_id),
    CONSTRAINT fk_request_id FOREIGN KEY (request_id)
        REFERENCES public.bitly_request (request_id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT fk_response_id FOREIGN KEY (response_id)
        REFERENCES public.bitly_response (response_id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
)

TABLESPACE pg_default;

ALTER TABLE public.short_link
    OWNER to postgres;

/* Functions */
/* func_read_all_short_links */
CREATE OR REPLACE FUNCTION public.func_read_all_short_links(
	OUT short_links short_link)
    RETURNS SETOF short_link 
    LANGUAGE 'sql'
    COST 100
    VOLATILE PARALLEL UNSAFE
    ROWS 1000

AS $BODY$
SELECT * FROM public.short_link
$BODY$;

ALTER FUNCTION public.func_read_all_short_links()
    OWNER TO postgres;

/* func_read_short_link */
CREATE FUNCTION public.func_read_short_link(OUT link short_link, IN link_id integer)
    RETURNS short_link
    LANGUAGE 'sql'
    
AS $BODY$
SELECT * FROM public.short_link
WHERE short_link_id = link_id
ORDER BY short_link_id
LIMIT 1
$BODY$;

ALTER FUNCTION public.func_read_short_link(integer)
    OWNER TO postgres;

/* func_write_short_link */
CREATE OR REPLACE FUNCTION public.func_write_short_link(
	shrt_link text,
	lng_link text,
	descr text,
	req_id integer,
	resp_id integer,
	OUT shrt_lnk_id integer)
    RETURNS integer
    LANGUAGE 'sql'
    COST 100
    VOLATILE PARALLEL UNSAFE
AS $BODY$
INSERT INTO public.short_link(
	short_link, long_link, description, request_id, response_id, date_added)
	VALUES (shrt_link, lng_link, descr, req_id, resp_id, NOW()) RETURNING short_link_id;
$BODY$;

ALTER FUNCTION public.func_write_short_link(text, text, text, integer, integer)
    OWNER TO postgres;

/* func_read_request */
CREATE OR REPLACE FUNCTION public.func_read_request(
	OUT request bitly_request,
	req_id integer)
    RETURNS bitly_request
    LANGUAGE 'sql'
    COST 100
    VOLATILE PARALLEL UNSAFE
AS $BODY$
SELECT * FROM bitly_request
WHERE request_id = req_id
ORDER BY request_id
LIMIT 1
$BODY$;

ALTER FUNCTION public.func_read_request(integer)
    OWNER TO postgres;

/* func_write_request */
CREATE FUNCTION public.func_write_request(IN req_string text, OUT req_id integer)
    RETURNS integer
    LANGUAGE 'sql'
    
AS $BODY$
INSERT INTO public.bitly_request(request_json)
	VALUES (req_string) RETURNING request_id;
$BODY$;

ALTER FUNCTION public.func_write_request(text)
    OWNER TO postgres;

/* func_write_response */
CREATE FUNCTION public.func_write_response(IN resp_string text, OUT resp_id integer)
    RETURNS integer
    LANGUAGE 'sql'
    
AS $BODY$
INSERT INTO public.bitly_response(response_json)
	VALUES (resp_string) RETURNING response_id;
$BODY$;

ALTER FUNCTION public.func_write_response(text)
    OWNER TO postgres;
