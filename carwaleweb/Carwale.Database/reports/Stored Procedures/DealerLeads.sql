IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[reports].[DealerLeads]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [reports].[DealerLeads]
GO

	-- =============================================
-- Author:		Avishkar
-- Create date: 3-2-2012
-- Description:	Dealer assigned leads in a month
-- =============================================
CREATE PROCEDURE [reports].[DealerLeads]
	-- Add the parameters for the stored procedure here
	@Dealer varchar(100),
	@City   varchar(100),
	@Mon int,
	@Year int
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	Declare @DealerId int,@CityId int
	
	select @DealerId = ID
	from Dealer_NewCar 
	where LTRIM(rtrim(Name))=LTRIM(rtrim(@Dealer))
	
	select  @DealerId = n.ID
	from Dealer_NewCar as n	 
	join cities as c on n.CityId=c.ID
	where LTRIM(rtrim(n.Name))=LTRIM(rtrim(@Dealer))
	and LTRIM(rtrim(c.Name))=LTRIM(rtrim(@City))
	
	--select @CityId = ID
	--from cities 
	--where LTRIM(rtrim(Name))=LTRIM(rtrim(@City))

   Select D.DealerId,D.NCD_Website,
     C.Name,C.Mobile,(vw.Make+' '+vw.Model+' '+vw.Version) as car,
     case InquirySource
          when 1 then 'Microsite'
          when 2 then 'CarWale'
     end as InquirySource,
     case RequestType
          when 1 then 'Price Quote'
          when 2 then 'Test Drive'
          when 4 then 'Loan Inquiry'
     end as RequestType,
     EntryDate as LeadDate,
    case IsAccepted 
        when 1 then 'Accepted' 
        when 0 then 'Rejected' 
    end as "Action",
    case IsAccepted 
        when 1 then isnull(LatestActionDesc,'Comment not available')
        when 0 then RejectionComment
    end "Feedback"
 from NCD_Inquiries as N
      join NCD_Dealers as D on N.DealerId=D.DealerId
      join Dealer_NewCar as DN on DN.Id=D.DealerId
      join Customers as C on N.CustomerId=C.Id
      join vwMMV as vw on vw.VersionId=N.VersionId
      --join Cities as Ct on C.Id=DN.CityId
where D.DealerId=@DealerId
--and DN.CityId=@CityId
and year(EntryDate)=@Year and month(Entrydate) =@Mon
and N.RequestType in (1,2,4) 
order by Entrydate desc
END
