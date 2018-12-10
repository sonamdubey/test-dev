IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetFreeFuelOfferWinner_15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetFreeFuelOfferWinner_15]
GO

	-- =============================================
-- Author:		Prachi Phalak
-- Create date: 12 oct , 2015
-- Description:	Retriving records of candidates from FreeFuelOfferCandidates table.
-- =============================================
CREATE PROCEDURE [dbo].[GetFreeFuelOfferWinner_15.10.3] 
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Id,Name,Email,Mobile,CWGTDPolicyNo,ReasonBuyingCWGTD AS Slogan,EntryDateTime,Iswinner,WinningDate,City FROM 
	FreeFuelOfferCandidates WITH(NOLOCK) 
	WHERE Iswinner = 1 
	AND CONVERT(DATE,GETDATE()-1) =CONVERT(DATE,WinningDate)

	SELECT Id,Name,Email,Mobile,CWGTDPolicyNo,ReasonBuyingCWGTD AS Slogan,EntryDateTime,Iswinner,WinningDate,City FROM 
	FreeFuelOfferCandidates WITH(NOLOCK)
	WHERE Iswinner = 1 
	AND CONVERT(DATE,GETDATE()-2) >= CONVERT(DATE,WinningDate)
	ORDER BY WinningDate DESC
	
END


