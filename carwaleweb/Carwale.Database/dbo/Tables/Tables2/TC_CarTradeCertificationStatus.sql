CREATE TABLE [dbo].[TC_CarTradeCertificationStatus] (
    [TC_CarTradeCertificationStatusId] TINYINT       IDENTITY (1, 1) NOT NULL,
    [Description]                      VARCHAR (100) NOT NULL,
    [IsActive]                         BIT           NOT NULL,
    CONSTRAINT [PK_TC_CarTradeCertificationStatus] PRIMARY KEY CLUSTERED ([TC_CarTradeCertificationStatusId] ASC)
);

