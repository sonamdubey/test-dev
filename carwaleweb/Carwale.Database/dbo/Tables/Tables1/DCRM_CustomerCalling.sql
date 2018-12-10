CREATE TABLE [dbo].[DCRM_CustomerCalling] (
    [ID]              INT          IDENTITY (1, 1) NOT NULL,
    [CustomerId]      NUMERIC (18) NULL,
    [ActionID]        INT          NULL,
    [EntryDate]       DATETIME     NULL,
    [EnteredBy]       INT          NULL,
    [ActionTakenBy]   INT          NULL,
    [ActionTakenOn]   DATETIME     NULL,
    [CCExecutive]     VARCHAR (50) NULL,
    [InquiryDate]     DATETIME     NULL,
    [IsFeedbackGiven] BIT          NULL
);

