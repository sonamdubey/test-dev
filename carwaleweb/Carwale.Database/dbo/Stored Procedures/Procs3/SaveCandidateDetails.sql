IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SaveCandidateDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SaveCandidateDetails]
GO

	CREATE PROCEDURE [dbo].[SaveCandidateDetails]
@DealerId numeric(18,0),
@Name varchar(50),
@EmailId varchar(50),
@MobileNumber varchar(15),
@PositionApplied varchar(150),
@LinkedinUrl varchar(250)
AS 
--Author:Rakesh yadav
--Date created: 28 april 2015
--Desc: save details of candidates applying for job to dealer
BEGIN
	INSERT INTO Microsite_Career
	(DealerId,Name,EmailId,MobileNumber,PositionApplied,LinkedinUrl,EntryDate) 
	VALUES 
	(@DealerId,@Name,@EmailId,@MobileNumber,@PositionApplied,@LinkedinUrl,GETDATE())
END