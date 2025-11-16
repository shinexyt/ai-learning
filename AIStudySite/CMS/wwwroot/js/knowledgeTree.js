window.aiStudyTree = {
    init: function (elementId, treeJson) {
        if (!window.cytoscape) {
            console.error('Cytoscape.js is required for the knowledge tree component.');
            return;
        }
        const container = document.getElementById(elementId);
        const nodes = [];
        const edges = [];

        function visit(node, parentId) {
            nodes.push({ data: { id: node.id, label: node.name, href: node.link } });
            if (parentId) {
                edges.push({ data: { source: parentId, target: node.id } });
            }
            (node.children || []).forEach(child => visit(child, node.id));
        }

        visit(treeJson, null);

        const cy = window.cytoscape({
            container,
            boxSelectionEnabled: false,
            autounselectify: true,
            layout: {
                name: 'breadthfirst',
                directed: true,
                padding: 20
            },
            style: [
                {
                    selector: 'node',
                    style: {
                        'content': 'data(label)',
                        'text-valign': 'center',
                        'color': 'var(--ai-foreground)',
                        'background-color': 'var(--ai-primary)',
                        'text-outline-width': 2,
                        'text-outline-color': 'var(--ai-surface)'
                    }
                },
                {
                    selector: 'edge',
                    style: {
                        'width': 2,
                        'line-color': 'var(--ai-border)',
                        'target-arrow-color': 'var(--ai-border)',
                        'target-arrow-shape': 'triangle'
                    }
                }
            ],
            elements: {
                nodes,
                edges
            }
        });

        cy.on('tap', 'node', evt => {
            const href = evt.target.data('href');
            if (href) {
                window.location.href = href;
            }
        });

        return cy;
    }
};
