--
-- PostgreSQL database dump
--

-- Dumped from database version 12.1
-- Dumped by pg_dump version 12.1

-- Started on 2020-04-19 13:47:19

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 205 (class 1259 OID 49288)
-- Name: BankResponse; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."BankResponse" (
    "Id" uuid NOT NULL,
    "PaymentId" uuid NOT NULL
);


ALTER TABLE public."BankResponse" OWNER TO postgres;

--
-- TOC entry 203 (class 1259 OID 49265)
-- Name: LoggedEvent; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."LoggedEvent" (
    "Id" uuid NOT NULL,
    "Action" text NOT NULL,
    "AggregateId" uuid NOT NULL,
    "Data" text NOT NULL,
    "TimeStamp" date NOT NULL
);


ALTER TABLE public."LoggedEvent" OWNER TO postgres;

--
-- TOC entry 204 (class 1259 OID 49273)
-- Name: Payment; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."Payment" (
    "Id" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    "CardNumber" text NOT NULL,
    "ExpiryMonth" integer NOT NULL,
    "ExpiryYear" smallint NOT NULL,
    "Amount" numeric NOT NULL,
    "CurrencyCode" character(3) NOT NULL,
    "CVV" character(3) NOT NULL
);


ALTER TABLE public."Payment" OWNER TO postgres;

--
-- TOC entry 202 (class 1259 OID 49257)
-- Name: UserAccount; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public."UserAccount" (
    "Id" uuid NOT NULL,
    "Username" text NOT NULL,
    "Password" text NOT NULL,
    "Salt" text NOT NULL
);


ALTER TABLE public."UserAccount" OWNER TO postgres;

--
-- TOC entry 2708 (class 2606 OID 49292)
-- Name: BankResponse BankResponse_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."BankResponse"
    ADD CONSTRAINT "BankResponse_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 2704 (class 2606 OID 49272)
-- Name: LoggedEvent LoggedEvent_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."LoggedEvent"
    ADD CONSTRAINT "LoggedEvent_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 2702 (class 2606 OID 49264)
-- Name: UserAccount PK_UserAccount; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."UserAccount"
    ADD CONSTRAINT "PK_UserAccount" PRIMARY KEY ("Id");


--
-- TOC entry 2706 (class 2606 OID 49280)
-- Name: Payment Payment_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."Payment"
    ADD CONSTRAINT "Payment_pkey" PRIMARY KEY ("Id");


--
-- TOC entry 2709 (class 2606 OID 49293)
-- Name: BankResponse FK_BankResponse_Payment_PaymentId; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public."BankResponse"
    ADD CONSTRAINT "FK_BankResponse_Payment_PaymentId" FOREIGN KEY ("PaymentId") REFERENCES public."Payment"("Id");


-- Completed on 2020-04-19 13:47:20

--
-- PostgreSQL database dump complete
--

