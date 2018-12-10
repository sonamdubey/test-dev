IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCD_Followup_Search]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCD_Followup_Search]
GO

	-- =============================================
-- Author:		<Umesh>
-- Create date: <10/10/2011>
-- Description:	<Return data with given search option as parameters in ncd_followup>
-- =============================================
CREATE PROCEDURE [dbo].[NCD_Followup_Search] 
	-- Add the parameters for the stored procedure here
	@dealerId int,
	@startIndex int,
	@endIndex int ,
	@Accepted bit,	
	@CustomerName Varchar(90)= null,
	@EmailId Varchar (50) = null,
	@ContactNumbner varchar(11) = null,
	@LeadStartDate datetime = null,
	@LeadEndDate datetime = null,
	@RejectStartDt Datetime = null,
	@RejectEndDt Datetime = null,
	@LeadType varchar(30) = null,
	@LeadSource varchar(8) = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON

    -- Insert statements for procedure here
	select TopRecords.* From(Select Top (@endIndex) Row_Number() Over (Order By CASE WHEN 
	@LeadStartDate IS NULL THEN NI.EntryDate ELSE NI.NextCallTime END DESC) AS RowN,	
	 NI.Id 'InquiryId',NI.CustomerId,NC.Name 'CustomerName',NC.Email,NC.Mobile,
	CM.Name+' '+CMD.NAME+' '+CV.Name 'Car',NI.LatestActionDesc,
	case when @Accepted = 1 then NI.NextCallTime
	else NI.EntryDate end as NextCallTime ,LOWER(NL.StatusName) 'statusType',
	case when NI.InquirySource=1 then 'Dealer Website'
	else 'CarWale' end as LeadSource 
	 from
	NCD_Customers NC INNER JOIN NCD_Inquiries NI ON NI.CustomerId=NC.Id
	LEFT OUTER JOIN NCD_LeadStatus NL ON NL.Id=NI.LeadStatusId
	LEFT OUTER JOIN CarVersions CV ON CV.ID=NI.VersionId 
	LEFT OUTER JOIN CarModels CMD ON CMD.ID=CV.CarModelId 
	LEFT OUTER JOIN CarMakes CM ON CM.ID=CMD.CarMakeId 
	where	
	NI.DealerId=@dealerId AND (@LeadStartDate IS NULL OR NI.RequestType in (1,2,4)) AND
	(@LeadStartDate IS NULL OR NI.NextCallTime <= GETDATE()) AND NI.IsActionTaken=1 AND NI.IsAccepted=@Accepted AND
	(@CustomerName IS NULL OR NC.Name like '%'+@CustomerName+'%')
	AND (@EmailId IS NULL OR NC.Email = @EmailId) and (@ContactNumbner IS NULL OR NC.Mobile = @ContactNumbner)
	AND (@LeadType IS NULL OR LOWER(NL.StatusName)=@LeadType) AND (@LeadSource IS NULL OR NI.InquirySource in (@LeadSource))
	AND (@LeadStartDate IS NULL OR NI.NextCallTime >= @LeadStartDate ) AND (@LeadEndDate IS NULL OR NI.NextCallTime <= @LeadEndDate )
	AND (@RejectStartDt IS NULL OR NI.EntryDate >= @RejectStartDt ) AND (@RejectEndDt IS NULL OR NI.EntryDate <= @RejectEndDt )
	order by
	CASE WHEN 
	@LeadStartDate IS NULL THEN NI.EntryDate ELSE NI.NextCallTime END DESC )AS TopRecords Where TopRecords.RowN between @startIndex AND @endIndex  
     
     
     -- Return No of rows from above query for procedure here
	Select COUNT( NI.Id ) as RowsReturned from
	NCD_Customers NC INNER JOIN NCD_Inquiries NI ON NI.CustomerId=NC.Id
	LEFT OUTER JOIN NCD_LeadStatus NL ON NL.Id=NI.LeadStatusId
	LEFT OUTER JOIN CarVersions CV ON CV.ID=NI.VersionId 
	LEFT OUTER JOIN CarModels CMD ON CMD.ID=CV.CarModelId 
	LEFT OUTER JOIN CarMakes CM ON CM.ID=CMD.CarMakeId 
	where
	NI.DealerId=5 AND (@LeadStartDate IS NULL OR NI.RequestType in (1,2,4)) AND
	(@LeadStartDate IS NULL OR NI.NextCallTime <= GETDATE()) AND NI.IsActionTaken=1 AND NI.IsAccepted=@Accepted AND
	(@CustomerName IS NULL OR NC.Name like '%'+@CustomerName+'%')
	AND (@EmailId IS NULL OR NC.Email = @EmailId) and (@ContactNumbner IS NULL OR NC.Mobile = @ContactNumbner)
	AND (@LeadType IS NULL OR LOWER(NL.StatusName)=@LeadType) AND (@LeadSource IS NULL OR NI.InquirySource in (@LeadSource))
	AND (@LeadStartDate IS NULL OR NI.NextCallTime >= @LeadStartDate ) AND (@LeadEndDate IS NULL OR NI.NextCallTime <= @LeadEndDate )
	AND (@RejectStartDt IS NULL OR NI.EntryDate >= @RejectStartDt ) AND (@RejectEndDt IS NULL OR NI.EntryDate <= @RejectEndDt )
	
END
