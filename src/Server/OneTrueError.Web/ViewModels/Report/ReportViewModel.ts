﻿/// <reference path="../../Scripts/Promise.ts" />
/// <reference path="../../Scripts/Griffin.WebApp.ts" />
/// <reference path="../../Scripts/CqsClient.ts" />
/// <reference path="../../Scripts/Griffin.Yo.d.ts" />
module OneTrueError.Report {
    import CqsClient = Griffin.Cqs.CqsClient;
    import ReportResult = Core.Reports.Queries.GetReportResult;

    export class ReportViewModel implements Griffin.Yo.Spa.ViewModels.IViewModel {
        private context: Griffin.Yo.Spa.ViewModels.IActivationContext;
        private dto: ReportResult;

        private renderView(): void {
            const directives = {
                CreatedAtUtc: {
                    text(value) {
                        return new Date(value).toLocaleString();
                    }
                },
                ContextCollections: {
                    ContextCollectionName: {
                        html(value, dto) {
                            return dto.Name;
                        },
                        href(value, dto) {
                            return `#${dto.Name}`;
                        }
                    }
                }
            };
            this.context.render(this.dto, directives);
        }

        getTitle(): string {
            return "Report";

        }

        activate(context: Griffin.Yo.Spa.ViewModels.IActivationContext): void {
            this.context = context;
            const reportId = context.routeData["reportId"];
            const query = new Core.Reports.Queries.GetReport(reportId);
            CqsClient.query<ReportResult>(query)
                .done(dto => {
                    this.dto = dto;
                    this.renderView();
                    context.resolve();
                });

            context.handle.click('[data-collection="ContextCollections"]',
                evt => {
                    evt.preventDefault();
                    var target = evt.target as HTMLElement;
                    if (target.tagName === "LI") {
                        this.selectCollection(target.firstElementChild.textContent);
                        $("li", target.parentElement).removeClass("active");
                        $(target).addClass("active");
                    } else if (target.tagName === "A") {
                        this.selectCollection(target.textContent);
                        $("li", target.parentElement.parentElement).removeClass("active");
                        $(target.parentElement).addClass("active");
                    }
                },
                true);

        }

        private selectCollection(collectionName: string) {
            this.dto.ContextCollections.forEach(item => {
                if (item.Name === collectionName) {
                    const directives = {
                        Properties: {
                            Key: {
                                html(value) {
                                    return value;
                                }
                            },
                            Value: {
                                html(value, dto) {
                                    if (collectionName === "Screenshots") {
                                        return `<img alt="Embedded Image" src="data:image/png;base64,${value}" />`;
                                    } else {
                                        return value.replace(/;;/g, "<br>");
                                    }
                                }
                            }
                        }
                    };
                    this.context.renderPartial("propertyTable", item, directives);
                    return;
                }
            });

        }


        deactivate() {}
    }
}