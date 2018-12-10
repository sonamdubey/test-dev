IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CheckAPVerification]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CheckAPVerification]
GO

	CREATE PROCEDURE [dbo].[CheckAPVerification]	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	create table #tempInquiries(
		Verificationdate int,
		Count int	
	)
	
    insert into #tempInquiries(Verificationdate,Count)
    select convert(varchar(8),verificationdate,112) as Verificationdate,Count(SellInqId) as count 
	from AP_VerifiedSellInq as AP WITH(NOLOCK)
	group by  convert(varchar(8),verificationdate,112)
	order by 1 desc
	
	select Verificationdate,Count
	from #tempInquiries
	
	
	DROP table #tempInquiries
END