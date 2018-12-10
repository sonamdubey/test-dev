CREATE TABLE [dbo].[PGVal] (
    [Id]            INT          IDENTITY (1, 1) NOT NULL,
    [Context]       VARCHAR (20) NULL,
    [SelliNquiryid] BIGINT       NULL,
    [updatedate]    DATETIME     DEFAULT (getdate()) NULL
);

