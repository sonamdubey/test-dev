CREATE TABLE [dbo].[InsuranceRules] (
    [RuleID]        INT IDENTITY (1, 1) NOT NULL,
    [ApplicationId] INT NULL,
    [PlatformId]    INT NULL,
    [LeadSource]    INT NULL,
    [CityId]        INT NULL,
    [State]         INT NULL,
    [MinPrice]      INT NULL,
    [MaxPrice]      INT NULL,
    [isNew]         BIT NULL,
    [MinAge]        INT NULL,
    [MaxAge]        INT NULL,
    [MinSalary]     INT NULL,
    [MaxSalary]     INT NULL,
    [IsActive]      BIT NULL,
    [ClientId]      INT NULL
);

