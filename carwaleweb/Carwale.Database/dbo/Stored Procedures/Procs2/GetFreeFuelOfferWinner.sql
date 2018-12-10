IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetFreeFuelOfferWinner]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetFreeFuelOfferWinner]
GO

	-- =============================================
-- Author:		Prachi Phalak
-- Create date: 12 oct , 2015
-- Description:	Retriving records of candidates from FreeFuelOfferCandidates table.
-- =============================================
CREATE PROCEDURE [dbo].[GetFreeFuelOfferWinner] 
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Id,Name,Email,Mobile,CWGTDPolicyNo,ReasonBuyingCWGTD AS Slogan,EntryDateTime,Iswinner,WinningDate 
	FROM 
	FreeFuelOfferCandidates WITH(NOLOCK) 
	WHERE Iswinner = 1 
	AND CONVERT(DATE,GETDATE()-1) =CONVERT(DATE,WinningDate)
	
END
