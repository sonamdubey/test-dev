IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCD_SelectQueries]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCD_SelectQueries]
GO

	
CREATE Proc [dbo].[NCD_SelectQueries]
(
@DealerId Int,
@EnqType int
--@FromDate datetime,
--@Todate datetime
)
as 
 select enq_cname 'Name',enq_cname 'Email',enq_mobile 'Mobile',enq_query 'Comment',
 convert(varchar(10),enq_req_datetime,103) 'RequestedDate'
 from NCD_Enquiries where enq_dealer_id=@DealerId and enq_req_type=@EnqType 
 and enq_deleted='N'
 --and enq_req_datetime between @FromDate and @Todate
 Order by enq_req_datetime desc

