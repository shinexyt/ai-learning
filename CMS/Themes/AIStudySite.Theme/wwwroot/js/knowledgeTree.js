(function () {
    function flatten(node, depth = 0, siblings = 1, index = 0, nodes = [], links = []) {
        if (!node) {
            return { nodes, links };
        }
        nodes.push({ id: node.id, label: node.label, url: node.url, depth, index, siblings });
        if (node.children && node.children.length) {
            node.children.forEach((child, childIndex) => {
                links.push({ from: node.id, to: child.id });
                flatten(child, depth + 1, node.children.length, childIndex, nodes, links);
            });
        }
        return { nodes, links };
    }

    function draw(element, data) {
        if (!data) {
            element.innerHTML = '<p class="text-error">Tree data is empty.</p>';
            return;
        }
        const { nodes, links } = flatten(data);
        const width = element.clientWidth || 800;
        const height = element.clientHeight || 480;
        const depthCount = Math.max(...nodes.map(n => n.depth)) + 1;
        const layerGap = height / (depthCount + 1);
        const coords = new Map();

        nodes.forEach(node => {
            const peers = nodes.filter(n => n.depth === node.depth);
            const xStep = width / (peers.length + 1);
            const x = xStep * (peers.indexOf(node) + 1);
            const y = layerGap * (node.depth + 1);
            coords.set(node.id, { x, y, node });
        });

        const svgParts = [
            `<svg viewBox="0 0 ${width} ${height}" preserveAspectRatio="xMidYMid meet">`
        ];

        links.forEach(link => {
            const from = coords.get(link.from);
            const to = coords.get(link.to);
            if (!from || !to) {
                return;
            }
            svgParts.push(`<line x1="${from.x}" y1="${from.y}" x2="${to.x}" y2="${to.y}" stroke="var(--border)" stroke-width="2"/>`);
        });

        nodes.forEach(entry => {
            const coord = coords.get(entry.id);
            if (!coord) {
                return;
            }
            svgParts.push(`<g class="tree-node" data-id="${entry.id}" data-url="${entry.url || ''}">`);
            svgParts.push(`<circle cx="${coord.x}" cy="${coord.y}" r="18" fill="var(--card)" stroke="var(--accent)" stroke-width="2"></circle>`);
            svgParts.push(`<text x="${coord.x}" y="${coord.y - 30}" text-anchor="middle" class="tree-label">${entry.label}</text>`);
            svgParts.push('</g>');
        });

        svgParts.push('</svg>');
        element.innerHTML = svgParts.join('');

        element.querySelectorAll('.tree-node').forEach(group => {
            group.addEventListener('click', () => {
                const url = group.getAttribute('data-url');
                if (url) {
                    window.location.href = url;
                }
            });
        });
    }

    window.aiStudyTree = {
        render: function (elementId, tree) {
            const element = document.getElementById(elementId);
            if (!element) {
                return;
            }
            draw(element, tree);
        }
    };
})();
