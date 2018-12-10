CREATE TABLE [dbo].[CRM_MonthlyPlan] (
    [Id]                NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [Plan_Date]         DATETIME      NOT NULL,
    [MakeId]            NUMERIC (18)  NOT NULL,
    [ModelId]           NVARCHAR (50) NULL,
    [LeadsInCrm]        INT           NOT NULL,
    [LeadsVerified]     INT           NOT NULL,
    [LeadsToConsultant] INT           NOT NULL,
    [LeadsAssign]       INT           NOT NULL,
    [PQ_Req]            INT           NOT NULL,
    [PQ_Delivr]         INT           NOT NULL,
    [TD_Req]            INT           NOT NULL,
    [TD_Delivr]         INT           NOT NULL,
    [Booking_Req]       INT           NOT NULL,
    [Booking_Comp]      INT           NOT NULL,
    [Created_By]        NVARCHAR (50) NOT NULL
);

